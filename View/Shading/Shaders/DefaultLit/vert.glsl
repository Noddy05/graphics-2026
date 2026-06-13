
#version 420 core

layout (location = 0) in vec3 iPosition;
layout (location = 1) in vec3 iNormal;
layout (location = 2) in vec2 iTextureCoordinate;

uniform mat4 transformationMatrix = mat4(1);
uniform mat4 projectionMatrix = mat4(1);
uniform mat4 cameraMatrix = mat4(1);

out vec3 vNormal;
out vec2 vTextureCoordinate;

layout (std140, binding = 1) uniform Camera
{
    mat4 view;
    mat4 projection;
} camera;  

void main(){
    vec3 worldPosition = (transformationMatrix * vec4(iPosition, 1)).xyz;

    vNormal = normalize((transformationMatrix * vec4(iNormal, 1) 
        - transformationMatrix * vec4(vec3(0), 1)).xyz);

    vTextureCoordinate = iTextureCoordinate;
    
    gl_Position = camera.projection * camera.view * vec4(worldPosition, 1);
}
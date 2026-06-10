
#version 330 core

layout (location = 0) in vec3 iPosition;
layout (location = 1) in vec3 iNormal;
layout (location = 2) in vec2 iTextureCoordinate;

uniform mat4 transformationMatrix = mat4(1);
uniform mat4 projectionMatrix = mat4(1);
uniform mat4 cameraMatrix = mat4(1);
uniform float textureScale = 1;

out vec3 vNormal;
out vec2 vTextureCoordinate;

void main(){
    vec3 worldPosition = (transformationMatrix * vec4(iPosition, 1)).xyz;
    vNormal = normalize(transformationMatrix * vec4(iNormal, 1) - transformationMatrix * vec4(vec3(0), 1)).xyz;
    vTextureCoordinate = worldPosition.xz / textureScale;
    
    gl_Position = projectionMatrix * cameraMatrix * vec4(worldPosition, 1);
}
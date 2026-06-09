
#version 330 core

layout (location = 0) in vec3 iPosition;

uniform mat4 transformationMatrix = mat4(1);
uniform mat4 projectionMatrix = mat4(1);
uniform mat4 cameraMatrix = mat4(1);

void main(){
    vec3 worldPosition = (transformationMatrix * vec4(iPosition, 1)).xyz;
    gl_Position = projectionMatrix * cameraMatrix * vec4(worldPosition, 1);
}
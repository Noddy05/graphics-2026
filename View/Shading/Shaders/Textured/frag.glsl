
#version 330 core
           

in vec3 vNormal;
in vec2 vTextureCoordinate;

uniform sampler2D textureSampler;

out vec4 oColor;

void main(){
    float lightLevel = max(0, dot(vNormal, vec3(0, 1, 0)));
    
    vec4 color = texture(textureSampler, vTextureCoordinate);

    oColor = color * lightLevel;
}
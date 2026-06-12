
#version 330 core
           

in vec3 vNormal;
in vec2 vTextureCoordinate;

uniform sampler2D textureSampler;

out vec4 oColor;

void main(){
    float lightLevel = max(0, dot(vNormal, normalize(vec3(-1, 2, 1.2))));
    
    vec3 color = texture(textureSampler, vTextureCoordinate).xyz;

    oColor = vec4(color * lightLevel, 1);
}
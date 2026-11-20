
#version 420 core
           

in vec2 vTextureCoordinate;

uniform sampler2D textureSampler;

out vec4 oColor;

void main(){
    oColor = texture(textureSampler, vTextureCoordinate);
}
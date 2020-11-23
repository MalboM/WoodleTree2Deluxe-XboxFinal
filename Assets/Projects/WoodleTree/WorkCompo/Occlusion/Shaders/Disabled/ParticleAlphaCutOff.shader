// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)
// Modified in order to disable Silhouette shader (Stencil Shader val.2)

Shader "Polymole/Particles/ParticleAlphaCutOffSilhouetteDisabled" {
Properties {
    _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
    _MainTex ("Particle Texture", 2D) = "white"
    _Stencil ("Stencil write", Int) = 2

}

SubShader {
    Tags { "Queue" = "Transparent" }
    Cull Off
    Lighting Off
    ZWrite On
    //Fog { color (0,0,0,0) }
    AlphaTest Greater .5
    ColorMask RGB
    BindChannels {
        Bind "Color", color
        Bind "Vertex", vertex
        Bind "TexCoord", texcoord
    }
    Pass {

        Stencil
        {
            Ref [_Stencil]
            Pass Replace
            Comp Always
        }

        SetTexture [_MainTex] {
            constantColor [_TintColor]
            combine constant * primary DOUBLE
        }
        SetTexture [_MainTex] {
            combine previous * texture
        }
    }
}
}

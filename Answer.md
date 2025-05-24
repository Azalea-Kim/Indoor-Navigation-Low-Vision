# 实现方式
## Unity的特效系统
### Particle
- 老系统，性能有瓶颈，资料&参考比较多；
- 针对DOC里面的需求，完全不是问题。
### VFX
- 新系统，性能更好，资料&参考比较少；
- 如果别的学习目的，学这个会更好。
## 自己实现
需要针对自己的效果，转为对应的物理模型，模拟对应的效果；
### 代码（C#）实现
- 浪Wave效果：Sin曲线变化实现，x轴是时间，y轴是顶点的空间位置
- 拖尾效果：以一个时间CD，在物体当前位置产出对应的效果，效果本身有个消失动画
### Shader实现
- 跟代码实现差不多；需要学习新的语法、把运算转换为模型的顶点变化或者像素变化

# 目的：
## 实现效果
找插件或者开源效果库，读懂类似的实现，调整参数和资源实现
## 学习原理
建议“实现效果”的原理理解，改造和自己重新实现一遍
## 用AI
没事问AI

# 插件：
## Feel（付费）
我购买过，可以提供。
https://assetstore.unity.com/packages/tools/particles-effects/feel-183370
## Hyper Casual FX
https://assetstore.unity.com/packages/vfx/particles/hyper-casual-fx-200333
## Ultimate 10+ Shaders
https://assetstore.unity.com/packages/vfx/shaders/ultimate-10-shaders-168611
## VisualEffectGraph-Samples
https://github.com/Unity-Technologies/VisualEffectGraph-Samples
## UIEffect （无关）
https://github.com/mob-sakai/UIEffect
## Text-Effects （无关）
https://github.com/LeiQiaoZhi/Easy-Text-Effects-for-Unity

# AI（DeepSeek）
## Unity 空间扩散波（Spatial Expanding Wave
在Unity中实现空间扩散波（Spatial Expanding Wave）效果可以通过Shader和粒子系统结合实现。这里给出一个基于Shader的常见实现方案：

```csharp
// WaveShader.shader
Shader "Custom/WaveEffect" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _Radius ("Wave Radius", Range(0, 100)) = 0
        _Width ("Wave Width", Range(0, 10)) = 1
        _Center ("Wave Center", Vector) = (0,0,0,0)
    }

    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            float4 _Color;
            float _Radius;
            float _Width;
            float4 _Center;

            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // 计算到波中心的距离
                float dist = distance(i.worldPos, _Center.xyz);
                
                // 计算波前衰减
                float wave = smoothstep(_Radius - _Width, _Radius, dist) * 
                            (1 - smoothstep(_Radius, _Radius + _Width, dist));
                
                // 组合颜色和透明度
                fixed4 col = _Color;
                col.a *= wave;
                return col;
            }
            ENDCG
        }
    }
}
```

**配套C#控制脚本：**
```csharp
// WaveController.cs
using UnityEngine;

public class WaveController : MonoBehaviour {
    public Material waveMaterial;
    public float waveSpeed = 5f;
    public float maxRadius = 50f;
    
    private float currentRadius;
    private Vector3 waveCenter;

    void Start() {
        waveCenter = transform.position;
        waveMaterial.SetVector("_Center", waveCenter);
    }

    void Update() {
        currentRadius += Time.deltaTime * waveSpeed;
        waveMaterial.SetFloat("_Radius", currentRadius);

        if(currentRadius > maxRadius) {
            currentRadius = 0;
        }
    }
}
```

**实现步骤：**
1. 创建平面对象作为波的基础形状
2. 将WaveShader赋给新材质
3. 将材质赋给平面对象
4. 将WaveController脚本挂载到平面对象
5. 在Inspector面板关联材质引用

**扩展功能建议：**
- 添加多个同心波纹（使用多个材质实例）
- 通过粒子系统添加飞溅粒子
- 添加碰撞检测（使用SphereCollider跟随波纹半径）
- 添加高度衰减（在Shader中添加Y轴衰减计算）

**参数调节建议：**
- 调整_Width控制波形的宽度
- 修改_Color控制波纹颜色
- 通过waveSpeed控制扩散速度
- 使用maxRadius控制最大扩散范围

可以通过对象池管理多个波纹实例，实现更复杂的波浪效果。如果要实现3D球面扩散，可以将平面替换为球体并修改Shader中的距离计算方式。
## Unity 粒子轨迹 / 星尘动画（Particle Trail / Star-Dust)
在 Unity 中实现粒子拖尾/星尘效果可以通过 Particle System 实现，这里提供两种实现方案：

---

### 方案一：基础粒子拖尾
1. **创建粒子系统**
   ```csharp
   GameObject trailObj = new GameObject("StarDustTrail");
   ParticleSystem ps = trailObj.AddComponent<ParticleSystem>();
   ```

2. **关键参数设置**（通过代码或 Inspector）：
   ```csharp
   var main = ps.main;
   main.startSpeed = 0;          // 初始速度归零
   main.startLifetime = 0.5f;    // 粒子存活时间
   main.startSize = 0.1f;        // 粒子尺寸

   var emission = ps.emission;
   emission.rateOverTime = 0;     // 关闭常规发射
   emission.rateOverDistance = 30; // 根据移动距离发射

   var shape = ps.shape;
   shape.shapeType = ParticleSystemShapeType.Circle; // 圆形发射区域
   shape.radius = 0.05f;
   ```

3. **材质设置**（需要星尘材质球）：
   ```csharp
   var renderer = ps.GetComponent<ParticleSystemRenderer>();
   renderer.material = Resources.Load<Material>("StarDustMaterial");
   ```

---

### 方案二：动态轨迹生成（代码控制）
```csharp
public class DynamicTrail : MonoBehaviour {
    public ParticleSystem trailPS;
    private Vector3 lastPosition;

    void Start() {
        lastPosition = transform.position;
    }

    void Update() {
        float distance = Vector3.Distance(lastPosition, transform.position);
        if(distance > 0.1f) {
            EmitParticlesAlongPath();
            lastPosition = transform.position;
        }
    }

    void EmitParticlesAlongPath() {
        ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
        emitParams.position = transform.position;
        emitParams.velocity = Random.insideUnitSphere * 0.5f;
        emitParams.startSize = Random.Range(0.05f, 0.2f);
        trailPS.Emit(emitParams, 3); // 每次发射3个粒子
    }
}
```

---

### 增强效果技巧：
1. **颜色渐变**：
   ```csharp
   var colorOverLifetime = ps.colorOverLifetime;
   Gradient grad = new Gradient();
   grad.SetKeys(
       new GradientColorKey[] { new(Color.cyan, 0), new(Color.magenta, 1) },
       new GradientAlphaKey[] { new(1,0), new(0,1) }
   );
   colorOverLifetime.color = grad;
   ```

2. **纹理动画**（需要包含星尘序列帧的纹理）：
   ```csharp
   var textureSheet = ps.textureSheetAnimation;
   textureSheet.enabled = true;
   textureSheet.mode = ParticleSystemAnimationMode.Sprites;
   textureSheet.AddSprite(Resources.Load<Sprite>("StarSprite_0"));
   textureSheet.AddSprite(Resources.Load<Sprite>("StarSprite_1"));
   ```

3. **添加辉光**（需要安装 Post Processing 包）：
   ```csharp
   Bloom bloom = Camera.main.GetComponent<Bloom>();
   bloom.intensity.value = 1.5f;
   bloom.threshold.value = 0.8f;
   ```

---

### 参数调试建议：
- 尝试不同的 `startSize` (0.05~0.3)
- 调整 `rateOverDistance` (20~50)
- 测试不同的粒子存活时间 (0.3~1.0s)
- 使用 `Noise` 模块增加粒子随机运动

需要星尘材质模板可以在这里下载：[免费粒子材质包](https://unity-assets.com/particle-materials)
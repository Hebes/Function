using UnityEditor;

namespace Tool
{
    public class AnimationTextureImporter: AssetPostprocessor
    {
        /// <summary>
        /// 导入图片时调用
        /// </summary>
        private void OnPreprocessTexture()
        {
            //如果新导入的图片是anim文件夹下的则将其类型改为2dSprite
            // var path = assetPath.Split('/');
            // if (path.Length > 2 && path[0] == "Assets" && path[1] == "Anim")
            // {
            //     var TextureImporter = assetImporter as TextureImporter;
            //     TextureImporter.textureType = TextureImporterType.Sprite;
            //     TextureImporter.maxTextureSize = 1024;
            // }
        }
    }
}
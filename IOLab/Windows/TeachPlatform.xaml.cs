using System.Windows;

namespace IOLab.Windows
{
    /// <summary>
    /// TeachPlatform.xaml 的交互逻辑
    /// </summary>
    public partial class TeachPlatform : Window
    {
        public TeachPlatform()
        {
            InitializeComponent();
            /*
            //ModelImporter import = new ModelImporter();//导入模型的类对象
            // 创建一个 3D 模型组
            ModelVisual3D visual3D = new ModelVisual3D();
            Model3DGroup modelGroup = new Model3DGroup();
            ObjReader CurrentHelixObjReader = new ObjReader();
            //相对路径，新建了一个objFile文件夹，添加了mtl，obj 和纹理图片
            modelGroup = CurrentHelixObjReader.Read(@"pack://application:,,,/Resources/3D/OBJ_PCB.obj");

            //显示灯光，否则模型都是显示为黑色,此处也可以直接在XAML中定义
            //DirectionalLight light = new DirectionalLight(Colors.White, new Vector3D(-1, -1, -1));
            DirectionalLight light1 = new DirectionalLight(Colors.White, new Vector3D(1, 1, 1));

            // 将光加入到模型组中
            //modelGroup.Children.Add(light);
            modelGroup.Children.Add(light1);

            // 创建一个绕 Y 轴的初始旋转变换
            /*
            var initialRotation = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 10));

            // 创建一个平移变换
            var translation = new TranslateTransform3D(new Vector3D(3, 0, 0));

            // 创建 Transform3DGroup 实例，并将旋转变换添加到其中
            var transformGroup = new Transform3DGroup();
            transformGroup.Children.Add(initialRotation);

            // 将 Transform3DGroup 应用到模型的 Transform 属性中
            modelGroup.Transform = transformGroup;

            // 创建一个平移动画，并设置其起始值、结束值和持续时间，循环次数属性
            var translationAnimation = new DoubleAnimation();
            translationAnimation.From = 0;
            translationAnimation.To = 90;
            translationAnimation.Duration = TimeSpan.FromSeconds(5);
            translationAnimation.RepeatBehavior = RepeatBehavior.Forever;


            // 将动画应用到平移变换的 OffsetX 属性上
            translation.BeginAnimation(TranslateTransform3D.OffsetXProperty, translationAnimation);

            // 将平移变换添加到 Transform3DGroup 中
            transformGroup.Children.Add(translation);

            //3D显示对象中加上内容->模型组， 动作->动画，
            visual3D = new ModelVisual3D { Content = modelGroup, Transform = translation };
            */

            /*
            visual3D = new ModelVisual3D { Content = modelGroup };
            // 创建 HelixViewport3D 控件，并将模型容器添加到其中
            viewPort3D.Children.Add(visual3D);
            */

        }
    }
}

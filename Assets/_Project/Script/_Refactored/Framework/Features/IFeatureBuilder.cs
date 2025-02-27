using Framework.Locators;
using Framework.Structures.Standard;

namespace Framework.Features
{
    public interface IFeatureBuilder<F> : IInitializableWithMini where F : IFeature
    {
        Locator<IFeature> FeatureLocator { get; }
    }
}
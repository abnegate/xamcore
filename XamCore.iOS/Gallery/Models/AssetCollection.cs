using System.Collections.Generic;
using System.Linq;
using Photos;

namespace XamCore.iOS
{
    public class AssetCollection
    {
        public string title;
        public List<Asset> assets = new List<Asset>();

        public AssetCollection(string title, PHFetchResult assets) 
        {
            this.title = title;
            foreach (PHAsset asset in assets) {
                this.assets.Add(new Asset(asset));
            }
        }

        public PHAsset FirstAsset() =>
            assets.First().assetRef; 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Extensions;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;

namespace ProSnippetsPointCloudSceneLayer
{

  #region ProSnippet Group: PointCloudSceneLayer
  #endregion

  class ProSnippetsPointCloudSceneLayer
  {

    public async void Examples()
    {
      // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer
      #region Name of PointCloudSceneLayer 
      var pcsl = MapView.Active.Map.GetLayersAsFlattenedList()
                     .OfType<PointCloudSceneLayer>().FirstOrDefault();
      var scenelayerName = pcsl?.Name;
      #endregion

      // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.GetDataSourceType
      // cref: ArcGIS.Desktop.Mapping.SceneLayerDataSourceType
      #region Get Data Source type for PointCloudSceneLayer
      //var pcsl = ...;
      //At 2.x - ISceneLayerInfo slInfo = pcsl as ISceneLayerInfo;
      //         SceneLayerDataSourceType dataSourceType = slInfo.GetDataSourceType();

      SceneLayerDataSourceType dataSourceType = pcsl.GetDataSourceType();
      if (dataSourceType == SceneLayerDataSourceType.Service)
      {
        //TODO...
      }
      else if (dataSourceType == SceneLayerDataSourceType.SLPK)
      {

      }
      #endregion

      await QueuedTask.Run(() =>
      {
        // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.GetAvailableClassCodesAndLabels
        #region Query all class codes and lables in the PointCloudSceneLayer
        //Must be called on the MCT
        //var pcsl = ...;
        //At 2.x - Dictionary<int, string> classCodesAndLabels =
        //                    pcsl.QueryAvailableClassCodesAndLabels();

        Dictionary<int, string> classCodesAndLabels = 
                        pcsl.GetAvailableClassCodesAndLabels();
        #endregion

        // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.GetAvailableClassCodesAndLabels
        // cref: ArcGIS.Desktop.Mapping.PointCloudFilterDefinition
        // cref: ArcGIS.Desktop.Mapping.PointCloudFilterDefinition.ClassCodes
        // cref: ArcGIS.Desktop.Mapping.PointCloudFilterDefinition.ReturnValues
        // cref: ArcGIS.Desktop.Mapping.PointCloudFilterDefinition.ToCIM
        // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.SetFilters
        // cref: ArcGIS.Core.CIM.PointCloudReturnType
        #region Set a Filter for PointCloudSceneLayer
        //Must be called on the MCT
        //var pcsl = ...;
        //Retrieve the available classification codes
        //At 2.x - var dict = pcsl.QueryAvailableClassCodesAndLabels();
        var dict = pcsl.GetAvailableClassCodesAndLabels();

        //Filter out low noise and unclassified (7 and 1 respectively)
        //consult https://pro.arcgis.com/en/pro-app/help/data/las-dataset/storing-lidar-data.htm
        var filterDef = new PointCloudFilterDefinition()
        {
          ClassCodes = dict.Keys.Where(c => c != 7 && c != 1).ToList(),
          ReturnValues = new List<PointCloudReturnType> { 
                                 PointCloudReturnType.FirstOfMany }
        };
        //apply the filter
        pcsl.SetFilters(filterDef.ToCIM());

        #endregion

        // cref: ArcGIS.Desktop.Mapping.PointCloudFilterDefinition.ClassFlags
        // cref: ArcGIS.Desktop.Mapping.PointCloudFilterDefinition.FromCIM
        // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.SetFilters
        // cref: ArcGIS.Desktop.Mapping.ClassFlag
        // cref: ArcGIS.Desktop.Mapping.ClassFlagOption
        #region Update the ClassFlags for PointCloudSceneLayer
        //Must be called on the MCT
        //var pcsl = ...;
        var filters = pcsl.GetFilters();
        PointCloudFilterDefinition fdef = null;
        if (filters.Count() == 0)
        {
          fdef = new PointCloudFilterDefinition()
          {
            //7 is "edge of flight line" - exclude
            ClassFlags = new List<ClassFlag> { 
               new ClassFlag(7, ClassFlagOption.Exclude) }
          };
        }
        else
        {
          fdef = PointCloudFilterDefinition.FromCIM(filters);
          //keep any include or ignore class flags
          var keep = fdef.ClassFlags.Where(
                 cf => cf.ClassFlagOption != ClassFlagOption.Exclude).ToList();
          //7 is "edge of flight line" - exclude
          keep.Add(new ClassFlag(7, ClassFlagOption.Exclude));
          fdef.ClassFlags = keep;
        }
        //apply
        pcsl.SetFilters(fdef.ToCIM());

        #endregion


        // cref: ArcGIS.Desktop.Mapping.PointCloudFilterDefinition.FromCIM
        // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.GetFilters
        // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.ReturnValues
        // cref: ArcGIS.Core.CIM.PointCloudReturnType
        // cref: ArcGIS.Core.CIM.CIMPointCloudFilter
        // cref: ArcGIS.Core.CIM.CIMPointCloudReturnFilter
        // cref: ArcGIS.Core.CIM.CIMPointCloudValueFilter
        // cref: ArcGIS.Core.CIM.CIMPointCloudBitFieldFilter
        #region Get the filters for PointCloudSceneLayer
        //Must be called on the MCT
        //var pcsl = ...;
        IReadOnlyList<CIMPointCloudFilter> updatedFilter = pcsl.GetFilters();
        foreach (var filter in updatedFilter)
        {
          //There is either 0 or 1 of each
          if (filter is CIMPointCloudReturnFilter returnFilter)
          {
            PointCloudFilterDefinition pcfl = PointCloudFilterDefinition.FromCIM(updatedFilter);
            List<PointCloudReturnType> updatedReturnValues = pcfl.ReturnValues;

          }
          if (filter is CIMPointCloudValueFilter classCodesFilter)
          {
            // do something
          }

          if (filter is CIMPointCloudBitFieldFilter classFlagsFilter)
          {
            // do something
          }
        }
        #endregion

        // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.ClearFilters
        #region Clear filters in PointCloudSceneLayer
        //Must be called on the MCT
        //var pcsl = ...;
        pcsl.ClearFilters();
        #endregion

        // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.GetRenderer
        // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.RendererType
        // cref: ArcGIS.Desktop.Mapping.PointCloudRendererType
        // cref: ArcGIS.Core.CIM.CIMPointCloudRenderer
        #region Get PointCloudSceneLayer Renderer and RendererType
        //Must be called on the MCT
        //var pcsl = ...;
        CIMPointCloudRenderer cimGetPCLRenderer = pcsl.GetRenderer();
        //Can be one of Unknown, Stretch, ClassBreaks, UniqueValue, RGB
        PointCloudRendererType pclRendererType = pcsl.RendererType;

        #endregion

        // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.GetAvailablePointCloudRendererFields
        // cref: ArcGIS.Desktop.Mapping.PointCloudRendererType
        #region Query PointCloudSceneLayer Renderer fields
        //Must be called on the MCT
        //var pcsl = ...;
        //At 2.x - IReadOnlyList<string> flds = pcsl.QueryAvailablePointCloudRendererFields(
        //                                     PointCloudRendererType.UniqueValueRenderer);
        IReadOnlyList<string> flds = pcsl.GetAvailablePointCloudRendererFields(
                                     PointCloudRendererType.UniqueValueRenderer);
        var fldCount = flds.Count;

        #endregion

        // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.GetAvailablePointCloudRendererFields
        // cref: ArcGIS.Desktop.Mapping.PointCloudRendererType
        // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.CreateRenderer
        // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.SetRenderer
        // cref: ArcGIS.Desktop.Mapping.ColorRampStyleItem
        // cref: ArcGIS.Desktop.Mapping.ColorRampStyleItem.ColorRamp
        // cref: ArcGIS.Core.CIM.CIMPointCloudStretchRenderer
        // cref: ArcGIS.Core.CIM.CIMPointCloudStretchRenderer.ColorRamp
        // cref: ArcGIS.Core.CIM.CIMPointCloudStretchRenderer.ColorModulation
        // cref: ArcGIS.Core.CIM.CIMColorRamp
        // cref: ArcGIS.Core.CIM.CIMColorModulationInfo
        // cref: ArcGIS.Core.CIM.CIMColorModulationInfo.MinValue
        // cref: ArcGIS.Core.CIM.CIMColorModulationInfo.MaxValue
        // cref: ARCGIS.DESKTOP.MAPPING.STYLEHELPER.SEARCHCOLORRAMPS
        #region Create and Set a Stretch Renderer
        //Must be called on the MCT
        //var pcsl = ...;

        //At 2.x - var fields = pcsl.QueryAvailablePointCloudRendererFields(
        //                           PointCloudRendererType.StretchRenderer);

        var fields = pcsl.GetAvailablePointCloudRendererFields(
                                 PointCloudRendererType.StretchRenderer);
        var stretchDef = new PointCloudRendererDefinition(
                                  PointCloudRendererType.StretchRenderer)
        {
          //Will be either ELEVATION or INTENSITY
          Field = fields[0]
        };
        //Create the CIM Renderer
        var stretchRenderer = pcsl.CreateRenderer(stretchDef) 
                                           as CIMPointCloudStretchRenderer;
        //Apply a color ramp
        var style = Project.Current.GetItems<StyleProjectItem>()
                                        .First(s => s.Name == "ArcGIS Colors");
        var colorRamp = style.SearchColorRamps("").First();
        stretchRenderer.ColorRamp = colorRamp.ColorRamp;
        //Apply modulation
        stretchRenderer.ColorModulation = new CIMColorModulationInfo()
        {
          MinValue = 0,
          MaxValue = 100
        };
        //apply the renderer
        pcsl.SetRenderer(stretchRenderer);

        #endregion
      });
    }

    public async void Examples1_b()
    {

      var pcsl = MapView.Active.Map.GetLayersAsFlattenedList().OfType<PointCloudSceneLayer>().FirstOrDefault();

      await QueuedTask.Run(() =>
      {
        // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.GetAvailablePointCloudRendererFields
        // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.CreateRenderer
        // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.SetRenderer
        // cref: ArcGIS.Desktop.Mapping.PointCloudRendererType
        // cref: ArcGIS.Desktop.Mapping.PointCloudRendererDefinition.Fields
        // cref: ArcGIS.Desktop.Mapping.ColorRampStyleItem
        // cref: ArcGIS.Desktop.Mapping.ColorRampStyleItem.ColorRamp
        // cref: ARCGIS.DESKTOP.MAPPING.STYLEHELPER.LOOKUPITEM
        // cref: ArcGIS.Desktop.Mapping.ColorFactory.GenerateColorsFromColorRamp
        // cref: ArcGIS.Core.CIM.CIMPointCloudClassBreaksRenderer
        // cref: ArcGIS.Core.CIM.CIMPointCloudClassBreaksRenderer.Breaks
        // cref: ArcGIS.Core.CIM.CIMColorClassBreak
        // cref: ArcGIS.Core.CIM.CIMColorClassBreak.UpperBound
        // cref: ArcGIS.Core.CIM.CIMColorClassBreak.Label
        // cref: ArcGIS.Core.CIM.CIMColorClassBreak.Color
        #region Create and Set a ClassBreaks Renderer
        //Must be called on the MCT
        //var pcsl = ...;

        //At 2.x - var fields = pcsl.QueryAvailablePointCloudRendererFields(
        //                          PointCloudRendererType.ClassBreaksRenderer);

        var fields = pcsl.GetAvailablePointCloudRendererFields(
                             PointCloudRendererType.ClassBreaksRenderer);
        var classBreakDef = new PointCloudRendererDefinition(
                                  PointCloudRendererType.ClassBreaksRenderer)
        {
          //ELEVATION or INTENSITY
          Field = fields[0]
        };
        //create the renderer
        var cbr = pcsl.CreateRenderer(classBreakDef) 
                                  as CIMPointCloudClassBreaksRenderer;
        //Set up a color scheme to use
        var style = Project.Current.GetItems<StyleProjectItem>()
                                   .First(s => s.Name == "ArcGIS Colors");
        var rampStyle = style.LookupItem(
          StyleItemType.ColorRamp, "Spectrum By Wavelength-Full Bright_Multi-hue_2")
                                                                    as ColorRampStyleItem;
        var colorScheme = rampStyle.ColorRamp;
        //Set up 6 manual class breaks
        var breaks = 6;
        var colors = ColorFactory.Instance.GenerateColorsFromColorRamp(
                                                    colorScheme, breaks);
        var classBreaks = new List<CIMColorClassBreak>();
        var min = cbr.Breaks[0].UpperBound;
        var max = cbr.Breaks[cbr.Breaks.Count() - 1].UpperBound;
        var step = (max - min) / (double)breaks;

        //add in the class breaks
        double upper = min;
        for (int b = 1; b <= breaks; b++)
        {
          double lower = upper;
          upper = b == breaks ? max : min + (b * step);
          var cb = new CIMColorClassBreak()
          {
            UpperBound = upper,
            Label = string.Format("{0:#0.0#} - {1:#0.0#}", lower, upper),
            Color = colors[b - 1]
          };
          classBreaks.Add(cb);
        }
        cbr.Breaks = classBreaks.ToArray();
        pcsl.SetRenderer(cbr);

        #endregion
      });
    }

    #region ProSnippet Group: PointCloudSceneLayer Extended Properties
    #endregion

    public async void Examples2()
    {

      var pcsl = MapView.Active.Map.GetLayersAsFlattenedList().OfType<PointCloudSceneLayer>().FirstOrDefault();

      await QueuedTask.Run(() =>
      {
        // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer
        // cref: ArcGIS.Desktop.Mapping.Layer.GetDefinition
        // cref: ArcGIS.Desktop.Mapping.Layer.SetDefinition
        // cref: ArcGIS.Core.CIM.CIMPointCloudLayer
        // cref: ArcGIS.Core.CIM.CIMPointCloudLayer.Renderer
        // cref: ArcGIS.Core.CIM.CIMPointCloudRenderer.ColorModulation
        // cref: ArcGIS.Core.CIM.CIMColorModulationInfo
        // cref: ArcGIS.Core.CIM.CIMColorModulationInfo.MinValue
        // cref: ArcGIS.Core.CIM.CIMColorModulationInfo.MaxValue
        #region Edit Color Modulation
        //Must be called on the MCT
        //var pcsl = ...;
        var def = pcsl.GetDefinition() as CIMPointCloudLayer;
        //Get the ColorModulation off the renderer
        var modulation = def.Renderer.ColorModulation;
        if (modulation == null)
          modulation = new CIMColorModulationInfo();
        //Set the minimum and maximum intensity as needed
        modulation.MinValue = 0;
        modulation.MaxValue = 100.0;
        //apply back
        def.Renderer.ColorModulation = modulation;
        //Commit changes back to the CIM
        pcsl.SetDefinition(def);

        #endregion
      });
    }

    public async void Examples3()
    {

      var pcsl = MapView.Active.Map.GetLayersAsFlattenedList().OfType<PointCloudSceneLayer>().FirstOrDefault();

      await QueuedTask.Run(() =>
      {
        // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer
        // cref: ArcGIS.Desktop.Mapping.Layer.GetDefinition
        // cref: ArcGIS.Desktop.Mapping.Layer.SetDefinition
        // cref: ArcGIS.Core.CIM.CIMPointCloudLayer.Renderer
        // cref: ArcGIS.Core.CIM.CIMPointCloudRenderer.PointSizeAlgorithm
        // cref: ArcGIS.Core.CIM.CIMPointCloudRenderer.PointShape
        // cref: ArcGIS.Core.CIM.PointCloudShapeType
        // cref: ArcGIS.Core.CIM.CIMPointCloudFixedSizeAlgorithm
        // cref: ArcGIS.Core.CIM.CIMPointCloudFixedSizeAlgorithm.UseRealWorldSymbolSizes
        // cref: ArcGIS.Core.CIM.CIMPointCloudFixedSizeAlgorithm.Size
        #region Edit The Renderer to use Fixed Size
        //Must be called on the MCT
        //var pcsl = ...;
        var def = pcsl.GetDefinition() as CIMPointCloudLayer;

        //Set the point shape and sizing on the renderer
        def.Renderer.PointShape = PointCloudShapeType.DiskShaded;
        var pointSize = new CIMPointCloudFixedSizeAlgorithm()
        {
          UseRealWorldSymbolSizes = false,
          Size = 8
        };
        def.Renderer.PointSizeAlgorithm = pointSize;
        //Commit changes back to the CIM
        pcsl.SetDefinition(def);
        #endregion

      });
      await QueuedTask.Run(() =>
      {
        // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer
        // cref: ArcGIS.Desktop.Mapping.Layer.GetDefinition
        // cref: ArcGIS.Desktop.Mapping.Layer.SetDefinition
        // cref: ArcGIS.Core.CIM.CIMPointCloudLayer.Renderer
        // cref: ArcGIS.Core.CIM.CIMPointCloudRenderer.PointSizeAlgorithm
        // cref: ArcGIS.Core.CIM.CIMPointCloudRenderer.PointShape
        // cref: ArcGIS.Core.CIM.PointCloudShapeType
        // cref: ArcGIS.Core.CIM.CIMPointCloudSplatAlgorithm
        // cref: ArcGIS.Core.CIM.CIMPointCloudSplatAlgorithm.MinSize
        // cref: ArcGIS.Core.CIM.CIMPointCloudSplatAlgorithm.ScaleFactor
        #region Edit the Renderer to Scale Size
        //Must be called on the MCT
        //var pcsl = ...;
        var def = pcsl.GetDefinition() as CIMPointCloudLayer;

        //Set the point shape and sizing on the renderer
        def.Renderer.PointShape = PointCloudShapeType.DiskFlat;//default
        var scaleSize = new CIMPointCloudSplatAlgorithm()
        {
          MinSize = 8,
          ScaleFactor = 1.0 //100%
        };
        def.Renderer.PointSizeAlgorithm = scaleSize;
        //Commit changes back to the CIM
        pcsl.SetDefinition(def);
        #endregion

      });
    }

    public async void Examples4()
    {

      var pcsl = MapView.Active.Map.GetLayersAsFlattenedList().OfType<PointCloudSceneLayer>().FirstOrDefault();

      await QueuedTask.Run(() =>
      {
        // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer
        // cref: ArcGIS.Desktop.Mapping.Layer.GetDefinition
        // cref: ArcGIS.Desktop.Mapping.Layer.SetDefinition
        // cref: ArcGIS.Core.CIM.CIMPointCloudLayer.PointsBudget
        // cref: ArcGIS.Core.CIM.CIMPointCloudLayer.PointsPerInch
        #region Edit Density settings
        //Must be called on the MCT
        //var pcsl = ...;
        var def = pcsl.GetDefinition() as CIMPointCloudLayer;
        //PointsBudget - corresponds to Display Limit on the UI
        // - the absolute maximum # of points to display
        def.PointsBudget = 1000000;

        //PointsPerInch - corresponds to Density Min --- Max on the UI
        // - the max number of points per display inch to renderer
        def.PointsPerInch = 15;
        //Commit changes back to the CIM
        pcsl.SetDefinition(def);

        #endregion

      });
    }
  }
}
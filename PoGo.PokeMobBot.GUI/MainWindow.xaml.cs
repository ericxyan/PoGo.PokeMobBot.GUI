#region usingdirectives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;
using Microsoft.Win32;

#if POKEMOBBOT
using System.Threading;
using PoGo.PokeMobBot.Logic;
using PoGo.PokeMobBot.CLI;
using PokemonGo.RocketAPI;
using PoGo.PokeMobBot.Logic.Common;
using PoGo.PokeMobBot.Logic.Event;
using PoGo.PokeMobBot.Logic.Logging;
using PoGo.PokeMobBot.Logic.State;
using PoGo.PokeMobBot.Logic.Tasks;
using PoGo.PokeMobBot.Logic.Utils;
#endif

#endregion

namespace PoGo.PokeMobBot.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Boolean initialized = false;

        public MainWindow()
        {
            InitializeComponent();
            textboxStatusPositionDescriptionData.Text = "Not Running";
            textblockStatusLog.Text = "Application Started: " + DateTime.Now.ToString();
            labelStatusRuntimeData.Content = "00:00:00.00";
            initialized = true;

#if POKEMOBBOT
            var settingsGlobal = GlobalSettings.Load("");            

            initializeSettings(settingsGlobal);
#endif

            tooltipsInitialize();                                           // Read the tooltips translation file
            initializeGlobalButtons();                                      // set the current state

            // cycle the controls to synchronize everything
            enableControls(false);
            enableControls(true);
            //

            MessageBoxResult messageboxExit = MessageBox.Show("This is a concept project.  It is not a functional part of the PokeMobBot project and is unrelated to the PokeMobBot project.  It is intended to be a GUI idea offered to the PokeMobBot project.  The buttons are interconnected but they are for display purposes only.", "This project is a concept only", MessageBoxButton.OK);
        }

#if POKEMOBBOT
        private void initializeSettings(GlobalSettings settings)
        {
            textboxLongitude.Text = settings.DefaultLongitude.ToString();
            textboxLatitude.Text = settings.DefaultLatitude.ToString();
            textboxAltitude.Text = settings.DefaultAltitude.ToString();
            checkboxUpdateWarning.IsChecked = settings.AutoUpdate;
            textboxDelayBetweenPlayerActions.Text = settings.DelayBetweenPlayerActions.ToString();
            textboxDelayBetweenPokemonCatch.Text = settings.DelayBetweenPokemonCatch.ToString();
            textboxMaxTravelDistanceInMeters.Text = settings.MaxTravelDistanceInMeters.ToString();
            textboxWalkingSpeedInKilometerPerHour.Text = settings.WalkingSpeedInKilometerPerHour.ToString();
            textboxKeepMinCp.Text = settings.KeepMinCp.ToString();
            textboxKeepMinDuplicatePokemon.Text = settings.KeepMinDuplicatePokemon.ToString();
            textboxKeepMinIvPercentage.Text = settings.KeepMinIvPercentage.ToString();
            textboxMaxBallsPerPokemon.Text = settings.MaxPokeballsPerPokemon.ToString();
            textboxUseGreatBallAboveIv.Text = settings.UseGreatBallAboveIv.ToString();
            textboxUseMasterBallAboveIv.Text = "";
            textboxUseUltraBallAboveIv.Text = "";
            checkboxUseEggIncubators.IsChecked = settings.UseEggIncubators;
            checkboxAutoFavoritePokemon.IsChecked = settings.AutoFavoritePokemon;
            checkboxTransferDuplicatePokemon.IsChecked = settings.TransferDuplicatePokemon;
            checkboxPrioritizeIvOverCP.IsChecked = settings.PrioritizeIvOverCp;
            checkboxKeepPokemonsThatCanEvolve.IsChecked = settings.KeepPokemonsThatCanEvolve;
            checkboxEvolveAllPokemonWithEnoughCandy.IsChecked = settings.EvolveAllPokemonWithEnoughCandy;
            checkboxEvolveAllPokemonAboveIv.IsChecked = settings.EvolveAllPokemonAboveIv;
            textboxEvolveAboveIvValue.Text = settings.EvolveAboveIvValue.ToString();
            checkboxRenamePokemon.IsChecked = settings.RenamePokemon;
            checkboxRenameAboveIv.IsChecked = settings.RenameOnlyAboveIv;
            textboxRenameTemplate.Text = settings.RenameTemplate.ToString();
            checkboxUseGpxPathing.IsChecked = settings.UseGpxPathing;
            textboxGpxFile.Text = settings.GpxFile.ToString();
            checkboxUseLuckyEggsWhileEvolving.IsChecked = settings.UseLuckyEggsWhileEvolving;
            textboxUseLuckyEggsMinPokemonAmount.Text = settings.UseLuckyEggsMinPokemonAmount.ToString();
            textboxMaxSpawnLocationOffset.Text = settings.MaxSpawnLocationOffset.ToString();
            if (settings.LevelUpByCPorIv.CompareTo("cp") == 0)
                comboboxLevelUpByCPorIV.SelectedItem = comboboxLevelUpByCPorIV_CP;
            else
                comboboxLevelUpByCPorIV.SelectedItem = comboboxLevelUpByCPorIV_IV;
            textboxUpgradePokemonCpMinimum.Text = settings.UpgradePokemonCpMinimum.ToString();
            textboxUpgradePokemonIvMinimum.Text = settings.UpgradePokemonIvMinimum.ToString();
            textboxUseBerryMinCp.Text = settings.UseBerryMinCp.ToString();
            textboxUseBerryMinIv.Text = settings.UseBerryMinIv.ToString();
            textboxUseGreatBallBelowCatchProbability.Text = settings.UseGreatBallBelowCatchProbability.ToString();
            textboxUseUltraBallAboveIv.Text = settings.UseUltraBallBelowCatchProbability.ToString();
            textboxUseMasterBallBelowCatchProbability.Text = settings.UseMasterBallBelowCatchProbability.ToString();
            textboxFavoriteMinIvPercentage.Text = settings.FavoriteMinIvPercentage.ToString();
            checkboxHumanizeThrows.IsChecked = settings.HumanizeThrows;
            textboxThrowAccuracyMin.Text = settings.ThrowAccuracyMin.ToString();
            textboxThrowAccuracyMax.Text = settings.ThrowAccuracyMax.ToString();
            textboxThrowSpinFrequency.Text = settings.ThrowSpinFrequency.ToString();
            textboxUseBerryBelowCatchProbability.Text = settings.UseBerryBelowCatchProbability.ToString();
            textboxRecycleInventoryAtUsagePercentage.Text = settings.RecycleInventoryAtUsagePercentage.ToString();
        }
#endif

        private void tooltipsInitialize()
        {
            // we are going to do this in code so that we are prepared for a change to another language through translations
            // putting these in statically just to demonstrate the idea
            buttonStart.ToolTip = "Start the Pokemon Bot from the location in the Settings tab";
            buttonContinue.ToolTip = "Start at the Longitude/Latitude where the Pokemon Bot previously stopped";
            buttonStop.ToolTip = "Stop/Pause the Pokemon Bot";
            buttonApply.ToolTip = "Apply setting changes to persistent configuration .json files";
            buttonExit.ToolTip = "Exit the Pokemon Bot";

            labelStatusLevel.ToolTip = "Current level of the logged in player";
            labelStatusXP.ToolTip = "Current experience level";
            labelStatusXPNextLevel.ToolTip = "Experience needed to get to the next level";
            labelStatusEXP.ToolTip = "";
            labelStatusPH.ToolTip = "";
            labelStatusStardust.ToolTip = "Current accumulated stardust";
            labelStatusTransferred.ToolTip = "Number of Pokemons transferred since starting";
            labelStatusRecycled.ToolTip = "Number of items (Potions, Pokeballs, etc.) that have been deleted";
            labelStatusLongitude.ToolTip = "Current Longitude when the application has started";
            labelStatusLatitude.ToolTip = "Current Lattitude when the application is running";

            labelAltitued.ToolTip = "Altitude that the bot starts at. Not advised to change this from the default as Niantic does not check altitude.";
            labelLatitude.ToolTip = "Latitude that the bot starts at. Must be between -90 and 90 values.";
            labelLongitude.ToolTip = "Longitude that the bot starts at. Must be between -180 and 180 values";
            labelWalkingSpeedInKilometerPerHour.ToolTip = "Walking speed in kilometers per hour.  Recomment setting to something that a human could perform (i.e. between 5 and 20)";
            labelMaxTravelDistanceInMeters.ToolTip = "How far the bot will travel in a radius from the original default location (in meters)";


        }

        private void radiobuttonChanged(object sender, RoutedEventArgs e)
        {

        }
        private void buttonFilenamePicker_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "GPX|*.gpx|All files|*.*";
            if (fileDialog.ShowDialog() == true)
            {
                textboxGpxFile.Text = fileDialog.FileName;
            }
        }

        private void comboboxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (initialized == false)         // we have to make this check because the combobox gets change gets called before the combobox is initialized
                return;

            if (sender == comboboxAuthType)
            {
                if (comboboxAuthType.SelectedItem == comboboxAuthType_Google)
                {
                    labelRefreshToken.Visibility = Visibility.Visible;
                    textboxRefreshToken.Visibility = Visibility.Visible;
                }
                else if (comboboxAuthType.SelectedItem == comboboxAuthType_Ptc)
                {
                    labelRefreshToken.Visibility = Visibility.Hidden;
                    textboxRefreshToken.Visibility = Visibility.Hidden;
                }
            }
        }

        private void enableControls(Boolean value)
        {
            gridRecycleFilter.IsEnabled = value;
            gridSetup.IsEnabled = value;
            gridMoreSetup.IsEnabled = value;
            gridExceptions.IsEnabled = value;            
            gridSnipeFilter.IsEnabled = value;
            gridTransferFilters.IsEnabled = value;
            buttonApply.IsEnabled = value;
            buttonExit.IsEnabled = value;
            buttonStart.IsEnabled = value;
            buttonContinue.IsEnabled = value;
            buttonStop.IsEnabled = (value == false);
        }

        private void initializeGlobalButtons()
        {
            buttonStart.IsEnabled = true;
            buttonStop.IsEnabled = false;
            buttonContinue.IsEnabled = true;
            buttonApply.IsEnabled = false;
            buttonExit.IsEnabled = true;
        }

        private void buttonClick(object sender, RoutedEventArgs e)
        {
            if (sender == buttonExit)
            {
                // need to check to see if any of the settings have changed.  If they have, present a dialog asking to save the settings

                // for now we will just ask for confirmation to exit
                MessageBoxResult messageboxExit = MessageBox.Show("Are you sure?", "Question", MessageBoxButton.OKCancel);
                if (messageboxExit == MessageBoxResult.OK)
                    Application.Current.Shutdown();
            }
            else if (sender == buttonApply)
            {
                // the settings in memory will be updated as they are changed by the user
                // the user will need to hit the apply button to cause the settings to be 
                // written back out to disk in the config.json and auth.json files
                MessageBoxResult messageboxApply = MessageBox.Show("Saving session configuration not implemented", "Notice", MessageBoxButton.OK);
            }
            else if (sender == buttonStart)
            {
                // first disable the controls so that settings cannot be changed while running the main thread
                enableControls(false);
                //
                // reset the longitude/latitued/altitude to the values from the .json files
                // create the main thread.  Events will be crated as information is sent and received from the PoGo servers
                //
                // main(args,nargs) /* Yea, I know that isn't what will be called */
            }
            else if (sender == buttonContinue)
            {
                enableControls(false);
                // same as buttonStart except do not reset the settings so that the bot starts from the longitude/latitude/altitude 
                // that it was last at
                // main(args,nargs) /* yea... bla bla bla */
            }
            else if (sender == buttonStop)
            {
                buttonStop.IsEnabled = false;
                buttonStart.IsEnabled = true;
                buttonContinue.IsEnabled = true;

                // kill the main thread.
                // then enable the controls
                enableControls(true);
            }
        }


        // this is a bit of a odd thing to do but I didn't want to replicate the code for enabling/disabling
        private void checkboxIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (initialized == false)
                return;

            checkboxChanged(sender, null);
        }

        private void checkboxChanged(object sender, RoutedEventArgs e)
        {
            if (initialized == false)  // we have to do this because we get a call with a null object on startup
                return;

            if (sender == checkboxUseGpxPathing)
            {
                if (checkboxUseGpxPathing.IsChecked == false || checkboxUseGpxPathing.IsEnabled == false)
                {
                    labelGpxFile.IsEnabled = false;
                    textboxGpxFile.IsEnabled = false;
                    buttonGpxFilePicker.IsEnabled = false;
                }
                else
                {
                    labelGpxFile.IsEnabled = true;
                    textboxGpxFile.IsEnabled = true;
                    buttonGpxFilePicker.IsEnabled = true;
                }
            }
            else if (sender == checkboxRenameAboveIv)
            {
                if (checkboxRenameAboveIv.IsChecked == false || checkboxRenameAboveIv.IsEnabled == false)
                {
                    textboxRenameTemplate.IsEnabled = false;
                    labelRenameTemplate.IsEnabled = false;
                }
                else
                {
                    textboxRenameTemplate.IsEnabled = true;
                    labelRenameTemplate.IsEnabled = true;
                }
            }
            else if (sender == checkboxTransferDuplicatePokemon)
            {
                if (checkboxTransferDuplicatePokemon.IsChecked == false || checkboxTransferDuplicatePokemon.IsEnabled == false)
                {
                    checkboxPrioritizeIvOverCP.IsEnabled = false;
                    checkboxPrioritizeIvOverCP.FontWeight = FontWeights.Normal;
                    textboxPokemonsNotToTransfer.IsEnabled = false;
                }
                else
                {
                    checkboxPrioritizeIvOverCP.IsEnabled = true;
                    checkboxPrioritizeIvOverCP.FontWeight = FontWeights.Bold;
                    textboxPokemonsNotToTransfer.IsEnabled = true;
                }
            }
            else if (sender == checkboxUseLuckyEggsWhileEvolving)
            {
                if (checkboxUseLuckyEggsWhileEvolving.IsChecked == false || checkboxUseLuckyEggsWhileEvolving.IsEnabled == false)
                {
                    textboxUseLuckyEggsMinPokemonAmount.IsEnabled = false;
                    labelUseLuckyEggsMinPokemonAmount.IsEnabled = false;
                }
                else
                {
                    textboxUseLuckyEggsMinPokemonAmount.IsEnabled = true;
                    labelUseLuckyEggsMinPokemonAmount.IsEnabled = true;
                }
            }
            else if (sender == checkboxRenamePokemon)
            {
                if (checkboxRenamePokemon.IsChecked == false || checkboxRenamePokemon.IsEnabled == false)
                {
                    checkboxRenameAboveIv.IsEnabled = false;
                    checkboxRenameAboveIv.FontWeight = FontWeights.Normal;
                    textboxRenameTemplate.IsEnabled = false;
                    labelRenameTemplate.IsEnabled = false;
                }
                else
                {
                    checkboxRenameAboveIv.IsEnabled = true;
                    checkboxRenameAboveIv.FontWeight = FontWeights.Bold;
                    textboxRenameTemplate.IsEnabled = true;
                    labelRenameTemplate.IsEnabled = true;
                }
            }
            else if (sender == checkboxSnipeAtPokestops)
            {
                if (checkboxSnipeAtPokestops.IsChecked == false || checkboxSnipeAtPokestops.IsEnabled == false)
                {
                    checkboxIgnoreUnknownIv.IsEnabled = false;
                    checkboxUseTransferIvForSnipe.IsEnabled = false;
                    labelMinDelayBetweenSnipes.IsEnabled = false;
                    textboxMinDelayBetweenSnipes.IsEnabled = false;
                    labelDelaySnipePokemon.IsEnabled = false;
                    textboxDelaySnipePokemon.IsEnabled = false;
                    labelMinPokeyballsToSnipe.IsEnabled = false;
                    textboxMinPokeballsToSnipe.IsEnabled = false;
                    labelMinPokeballsWhileSnipe.IsEnabled = false;
                    textboxMinPokeballsWhileSnipe.IsEnabled = false;
                    checkboxUseSnipeLocationServer.IsEnabled = false;
                    listviewPokemonToSnipe.IsEnabled = false;
                    textboxPokemonToSnipe.IsEnabled = false;
                }
                else
                {
                    checkboxIgnoreUnknownIv.IsEnabled = true;
                    checkboxUseTransferIvForSnipe.IsEnabled = true;
                    labelMinDelayBetweenSnipes.IsEnabled = true;
                    textboxMinDelayBetweenSnipes.IsEnabled = true;
                    labelDelaySnipePokemon.IsEnabled = true;
                    textboxDelaySnipePokemon.IsEnabled = true;
                    labelMinPokeyballsToSnipe.IsEnabled = true;
                    textboxMinPokeballsToSnipe.IsEnabled = true;
                    labelMinPokeballsWhileSnipe.IsEnabled = true;
                    textboxMinPokeballsWhileSnipe.IsEnabled = true;
                    checkboxUseSnipeLocationServer.IsEnabled = true;
                    listviewPokemonToSnipe.IsEnabled = true;
                    textboxPokemonToSnipe.IsEnabled = true;
                }
            }
            else if (sender == checkboxUseSnipeLocationServer)
            {
                if (checkboxUseSnipeLocationServer.IsChecked == false || checkboxUseSnipeLocationServer.IsEnabled == false)
                {
                    labelSnipeLocationServer.IsEnabled = false;
                    textboxSnipeLocationServer.IsEnabled = false;
                    labelSnipeLocationServerPort.IsEnabled = false;
                    textboxSnipeLocationServerPort.IsEnabled = false;
                }
                else
                {
                    labelSnipeLocationServer.IsEnabled = true;
                    textboxSnipeLocationServer.IsEnabled = true;
                    labelSnipeLocationServerPort.IsEnabled = true;
                    textboxSnipeLocationServerPort.IsEnabled = true;
                }
            }
            else if (sender == checkboxEvolveAllPokemonAboveIv)
            {
                if (checkboxEvolveAllPokemonAboveIv.IsChecked == false || checkboxEvolveAllPokemonAboveIv.IsEnabled == false)
                {
                    labelEvolveAboveIvValue.IsEnabled = false;
                    textboxEvolveAboveIvValue.IsEnabled = false;
                }
                else
                {
                    labelEvolveAboveIvValue.IsEnabled = true;
                    textboxEvolveAboveIvValue.IsEnabled = true;

                }
            }
            else if (sender == checkboxHumanizeThrows)
            {
                if (checkboxHumanizeThrows.IsChecked == false || checkboxHumanizeThrows.IsEnabled == false)
                {
                    labelThrowAccuracyMin.IsEnabled = false;
                    textboxThrowAccuracyMin.IsEnabled = false;
                    labelThrowAccuracyMax.IsEnabled = false;
                    textboxThrowAccuracyMax.IsEnabled = false;
                    labelThrowSpinFrequency.IsEnabled = false;
                    textboxThrowSpinFrequency.IsEnabled = false;
                }
                else
                {
                    labelThrowAccuracyMin.IsEnabled = true;
                    textboxThrowAccuracyMin.IsEnabled = true;
                    labelThrowAccuracyMax.IsEnabled = true;
                    textboxThrowAccuracyMax.IsEnabled = true;
                    labelThrowSpinFrequency.IsEnabled = true;
                    textboxThrowSpinFrequency.IsEnabled = true;
                }
            }
            else if (sender == checkboxUsePokemonNotToCatchFilter)
            {
                if (checkboxUsePokemonNotToCatchFilter.IsChecked == false || checkboxUsePokemonNotToCatchFilter.IsEnabled == false)
                {
                    listboxPokemonsToIgnore.IsEnabled = false;
                }
                else
                {
                    listboxPokemonsToIgnore.IsEnabled = true;
                }
            }
            else if (sender == checkboxAutoFavoritePokemon)
            {
                if (checkboxAutoFavoritePokemon.IsChecked == false || checkboxAutoFavoritePokemon.IsEnabled == false)
                {
                    labelFavoriteMinIvPercentage.IsEnabled = false;
                    textboxFavoriteMinIvPercentage.IsEnabled = false;
                }
                else
                {
                    labelFavoriteMinIvPercentage.IsEnabled = true;
                    textboxFavoriteMinIvPercentage.IsEnabled = true;
                }
            }
        }
    }
}

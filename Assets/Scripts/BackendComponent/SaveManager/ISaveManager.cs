namespace Assets.Scripts.BackendComponent.SaveManager
{
    public interface ISaveManager
    {
        /// <summary>
        /// Update mission status for chapter after mission is passed.
        /// </summary>
        /// <param name="missionFolderPath">Folder path for mission config file in seleted chapter and must be after 'Resources' folder sush as 'MissionConfigs/ChapterX'</param>
        /// <param name="passedMission">Mission name that passed.</param>
        void UpdateMissionStatus(string missionFolderPath, string passedMission);
    }
}

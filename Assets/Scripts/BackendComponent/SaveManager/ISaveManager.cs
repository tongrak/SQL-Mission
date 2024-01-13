namespace Assets.Scripts.BackendComponent.SaveManager
{
    public interface ISaveManager
    {
        /// <summary>
        /// Update mission status for chapter after mission is passed.
        /// </summary>
        /// <param name="missionFolderPathAfterResources">Folder path for mission config file in seleted chapter and must be after 'Resources' folder sush as 'MissionConfigs/ChapterX'</param>
        /// <param name="passedMissionName">Mission name that passed.</param>
        void UpdateMissionStatus(string missionFolderPathAfterResources, string passedMissionName, string[] missionDependTos);

        ///// <summary>
        ///// Update chapter status.
        ///// </summary>
        ///// <param name="chapterFolderPath">Folder path for chapter config file in seleted chapter and must be after 'Resources' folder sush as 'MissionConfigs/ChapterX'</param>
        ///// <param name="passedChapterName">Chapter name that passed.</param>
        //void UpdateChapterStatus(string chapterFolderPath, string passedChapterName);
    }
}

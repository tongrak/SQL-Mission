using Assets.Scripts.DataPersistence.ChapterStatusDetail;
using Assets.Scripts.DataPersistence.MissionStatusDetail;

namespace Assets.Scripts.DataPersistence.SaveManager
{
    public interface ISaveManager
    {
        /// <summary>
        /// Update mission status for chapter after mission is passed.
        /// </summary>
        /// <param name="missionFolderFullPath">Inside path must contain 'Resources/' folder such as "D:X/X/X/Resources/X/X/Chapter1"</param>
        /// <param name="passedMissionID">Mission ID that passed.</param>
        MissionUnlockDetails UpdateMissionStatus(string missionFolderFullPath, MissionUnlockDetails missionStatusDetails, int passedMissionID);

        /// <summary>
        /// Update chapter status.
        /// </summary>
        /// <param name="chapterFolderFullPath">Inside path must contain 'Resources/' folder such as "D:X/X/X/Resources/X/X/Chapter1"</param>
        /// <param name="passedChapterID">Chapter ID that passed.</param>
        ChapterStatusDetails UpdateChapterStatus(string chapterFolderFullPath, ChapterStatusDetails chapterStatusDetails, int passedChapterID, bool saveToFile);
    }
}

//***
// Author: Nate
// Description: LevelDataEditor.cs is a custom editor that draws a the level data in the inspector 
//***

using FoxHerding.Levels;
using UnityEditor;

namespace FoxHerding
{
    [CustomEditor(typeof(LevelData))]
    public class LevelDataEditor : UnityEditor.Editor
    {
        private SerializedProperty levelGridProp;
        private SerializedProperty availablePiecesProp;
        private SerializedProperty introduceNewPieceProp;
        private SerializedProperty pieceToIntroduceProp;

        private void OnEnable()
        {
            levelGridProp = serializedObject.FindProperty("LevelGrid");
            availablePiecesProp = serializedObject.FindProperty("AvailablePieces");
            introduceNewPieceProp = serializedObject.FindProperty("IntroduceNewPiece");
            pieceToIntroduceProp = serializedObject.FindProperty("PieceToIntroduce");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (levelGridProp != null)
            {
                EditorGUILayout.PropertyField(levelGridProp);
            }
            else
            {
                EditorGUILayout.HelpBox("LevelGrid property not found.", MessageType.Error);
            }

            if (availablePiecesProp != null)
            {
                EditorGUILayout.PropertyField(availablePiecesProp);
            }
            else
            {
                EditorGUILayout.HelpBox("AvailablePieces property not found.", MessageType.Error);
            }

            if (introduceNewPieceProp != null)
            {
                EditorGUILayout.PropertyField(introduceNewPieceProp);

                if (introduceNewPieceProp.boolValue && pieceToIntroduceProp != null)
                {
                    EditorGUILayout.PropertyField(pieceToIntroduceProp);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("IntroduceNewPiece property not found.", MessageType.Error);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SideQuestGenerator
{
    public class CharactersDebug : MonoBehaviour
    {
        public bool Debug;

        private bool m_curDebug;
        // Start is called before the first frame update
        void Start()
        {
            var cdus = GetComponentsInChildren<CharacterDebugUI>();
            m_curDebug = Debug;
            foreach (var item in cdus)
            {
                item.Debug = m_curDebug;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Debug != m_curDebug)
            {
                var cdus = GetComponentsInChildren<CharacterDebugUI>();
                m_curDebug = Debug;
                foreach (var item in cdus)
                {
                    item.Debug = m_curDebug;
                }
            }
        }
    }
}
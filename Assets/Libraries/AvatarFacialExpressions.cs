using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AvatarFacialExpressions {
    /// <summary>
    /// The BlendShape class is a data-structure that holds the Name, SkinnedMeshRenderer, and Index value of a blend
    /// shape.
    /// </summary>
    internal class BlendShape {
        public string Name { get; }
        public SkinnedMeshRenderer Mesh { get; }
        public int Index { get; }

        public BlendShape(string name, SkinnedMeshRenderer mesh, int index) {
            Name = name;
            Mesh = mesh;
            Index = index;
        }
    }

    /// <summary>
    /// The BlendShapeController class parses all blend shapes on a mesh and makes it easy to get the existing blend
    /// shapes as well as setting the intensity of each of them.
    /// </summary>
    internal class BlendShapeController {
        private readonly BlendShape[] _blendShapes;

        /// <summary>
        /// The constructor is responsible for parsing the blend shapes of a list of SkinnedMeshRenderer. The
        /// SkinnedMeshRenderer is the object that contains blend shapes. A game object can have several child
        /// SkinnedMeshRenderers therefore it takes a list of all the components that should be parsed.
        /// </summary>
        /// <param name="components">A list of SkinnedMeshRenderers that should be parsed.</param>
        public BlendShapeController(IReadOnlyList<SkinnedMeshRenderer> components) {
            // We create an array of all the meshes and add the collected meshes to said array
            var meshes = new SkinnedMeshRenderer[components.Count];
            for (var i = 0; i < components.Count; i++)
                meshes[i] = components[i];

            // We need to know how many blend shapes there are so we loop through each mesh and get the number of blend
            // shapes. We can then create an array of that size.
            var totalBlendShapeCount = meshes.Sum(mesh => mesh.sharedMesh.blendShapeCount);
            _blendShapes = new BlendShape[totalBlendShapeCount];

            // Now we once again loop through all blend shapes in each mesh and create a BlendShape instance to keep 
            // track of the name, mesh its attached to, and the index which will all be used to set the weight later.
            var index = 0;
            foreach (var mesh in meshes) {
                var sharedMesh = mesh.sharedMesh;
                for (var i = 0; i < sharedMesh.blendShapeCount; i++) {
                    var blendShapeName = sharedMesh.GetBlendShapeName(i);
                    _blendShapes[index] = new BlendShape(blendShapeName, mesh, i);
                    index++;
                }
            }
        }

        /// <summary>
        /// Returns a BlendShape instance if it exists by name.
        /// </summary>
        /// <param name="name">The name of the blend shape to get.</param>
        /// <returns></returns>
        private List<BlendShape> GetBlendShape(string name) {
            // Return the blend shape if it exists, otherwise return null
            return _blendShapes.Where(blendShape => blendShape.Name == name).ToList();
        }

        /// <summary>
        /// Sets a blend shapes weight to value
        /// </summary>
        /// <param name="name">The name of the blend shape.</param>
        /// <param name="value">The weight, a value from 0 to 100. With 100 being to highest weight.</param>
        /// <returns>boolean value if the blend shape was set or not.</returns>
        public bool SetBlendShape(string name, float value) {
            // Set the blend shape if it exists. Note: if there are two blend shapes with the same name they will both
            // be set.
            var targets = GetBlendShape(name);
            if (targets.Count == 0) {
                return false;
            }

            foreach (var target in targets) {
                target?.Mesh.SetBlendShapeWeight(target.Index, value);
            }

            return true;
        }

        /// <summary>
        /// Returns an array containing all the blend shape names.
        /// </summary>
        /// <returns>Array of blend shape names.</returns>
        public string[] GetBlendShapeNames() {
            // Returns a list of all blend shape names.
            var names = new string[_blendShapes.Length];
            for (var i = 0; i < names.Length; i++) {
                names[i] = _blendShapes[i].Name;
            }

            return names.Distinct().ToArray(); // we remove any duplicate values
        }
    }

    /// <summary>
    /// The Emotion class is a data-structure containing an emotion name, which corresponds to a blend shape. 
    /// </summary>
    internal class Emotion {
        public string Category { get; }
        public string SubCategory { get; }
        public int Variation { get; }
        public string BlendShapeName { get; }

        public Emotion(string category, string subCategory, int variation, string blendShapeName) {
            Category = category;
            SubCategory = subCategory;
            Variation = variation;
            BlendShapeName = blendShapeName;
        }
    }

    /// <summary>
    /// The EmotionController class is responsible for setting an Avatar's emotions. It parses blend shapes and
    /// implements setting the blend shapes based on an emotion name.
    /// </summary>
    public class EmotionController {
        /*
         *  Blend Shape parsing:
         *      TODO: check if the blend shape contains "emotion" or some other identifier
         *      1. emotion name. e.g. "anger", "fear", etc.
         *      2. part of the face. e.g. "brows", "eyes", "mouth", etc.
         *      3. variation. e.g. "a", "b", "c", etc.
         *
         *  Split all blend keys into categories a.k.a. the emotion name. That way we can easily access all blend
         *  shapes that belong together.
         *
         *  Now that we have grouped the emotions, we need a way to control all the individual components of the
         *  emotion, e.g. the eyes, the eyebrows, the mouth, etc. So we group each blend shape into these sub
         *  categories. If two blend shapes should be controlled together (e.g. moving the mouth, the tongue,
         *  and the teeth together) they should have the EXACT same name. This way you can't open the mouth but
         *  not move the teeth as well. Now individual components can be controlled to match a specific intensity.
         *
         *  Emotion variations: sometimes the same emotion can be expressed in multiple different ways (e.g.
         *  sometimes the mouth is open wide while otherwise its only slightly open). Therefore we add a label to
         *  indicate that this blend shape is a variation. These two blend shapes should never be expressed together
         *  at the same time otherwise we could accidentally deform the face in a very unnatural way. These variations
         *  are labelled from "a" to "z".
         *  TODO: we need to determine how the user can specific how these variations are used (i.e. the variations 
         *  TODO: are random, tend to be more variation "a" or some other way.
         *
         *  Finally we need to be able to blend different emotions together. If a avatar needs to express fear but
         *  also surprise we should be able to fix the intensities together to for both emotions. Most likely the
         *  individual components can only express one of the emotion. The fear emotion is expressed with the eyebrows
         *  and the surprise is in the mouth. TODO: there probably needs to be a way to prevent both emotions using
         *  TODO: the same exact expression. 
         */

        private readonly BlendShapeController _controller;

        private readonly Dictionary<string, List<Emotion>> _emotions;
        private readonly Dictionary<string, float> _emotionsValues;

        /// <summary>
        /// Creating an instance requires the a game object which is the avatar. It collects all its blend shapes
        /// and parses the blend shapes.
        /// </summary>
        /// <param name="attachToObject">The object that is the avatar.</param>
        public EmotionController(GameObject attachToObject) {
            // The blend shape controller will actually set the blend shape values
            var collectedChildComponents = attachToObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            _controller = new BlendShapeController(collectedChildComponents);

            // This dictionary stores the current emotion values
            _emotionsValues = new Dictionary<string, float>();

            // This function will parse the blend shapes to emotions
            _emotions = ParseEmotionBlendShapes();
        }

        /// <summary>
        /// This function loops through all the blend shapes and parses them based on name.
        /// </summary>
        /// <returns>Dictionary with emotion name as key and list of Emotion instances as value.</returns>
        private Dictionary<string, List<Emotion>> ParseEmotionBlendShapes() {
            var names = _controller.GetBlendShapeNames();

            // We first parse all the emotions that exist and add them to a list
            var emotions = new Dictionary<string, List<Emotion>>();
            foreach (var name in names) {
                var s = name.Split('-');
                if (!emotions.ContainsKey(s[0])) {
                    emotions.Add(s[0], new List<Emotion>());
                }

                if (!_emotionsValues.ContainsKey(s[0])) {
                    _emotionsValues.Add(s[0], 0f);
                }
            }

            foreach (var name in names) {
                var s = name.Split('-');
                string subCategory = null;
                var variation = 0;

                // double check that the sub category is actually present
                if (s.Length > 1)
                    subCategory = s[1];

                // this value is optional so we need to check if it exists
                if (s.Length > 2) {
                    // The variation is a letter from `a` to `z`. To convert this to numeric value we get the ascii
                    // value and subtract 97 (the ascii value for `a`). This will give us a value of 0 for `a`, 1 for
                    // `b`, 2 for `c`, etc. This this obviously not work when the variation is not a letter from a to 
                    // z and this needs to be coded as well (TODO)
                    var asciiBytes = Encoding.ASCII.GetBytes(s[2]);
                    if (asciiBytes.Length > 0) {
                        variation = asciiBytes[0] - 97;
                    }
                }

                emotions[s[0]].Add(new Emotion(s[0], subCategory, variation, name));
            }

            return emotions;
        }

        /// <summary>
        /// The SetEmotion function sets the blend shapes that correspond to an emotion name. It will set all blend
        /// shapes for a given emotion. It takes 500ms to set an emotion.
        /// </summary>
        /// <param name="emotionName">The name of the emotion. E.g. "anger".</param>
        /// <param name="intensity">A value from 0 to 100. A higher value means the blend shapes have a greater weight</param>
        /// <returns></returns>
        public IEnumerator SetEmotion(string emotionName, float intensity) {
            if (!_emotions.ContainsKey(emotionName)) {
                yield return false;
            }

            // We need to keep track of the current intensity and set the new intensity
            var currentIntensity = _emotionsValues[emotionName];
            _emotionsValues[emotionName] = intensity;

            var difference = intensity - currentIntensity;
            difference = Math.Abs(difference);

            var incrementBy = 1;
            if (difference >= 50) incrementBy = 2;

            var waitInterval = .5f / difference;

            for (int i = 0; i <= difference; i += incrementBy) {
                // Universally set each blend shape with the emotion name to the same intensity
                // TODO: use the variations

                foreach (var emotion in _emotions[emotionName]) {
                    if (emotion.Variation == 0) {
                        _controller.SetBlendShape(emotion.BlendShapeName, currentIntensity + i);
                    }
                }

                yield return new WaitForSeconds(waitInterval);
            }

            yield return true;
        }

        /// <summary>
        /// Get an emotion's current intensity
        /// </summary>
        /// <param name="emotionName">Name of the emotion.</param>
        /// <returns>The intensity value of the current emotion.</returns>
        public float GetEmotionIntensity(string emotionName) {
            if (!_emotions.ContainsKey(emotionName)) {
                return 0;
            }

            return _emotionsValues[emotionName];
        }
    }

    /// <summary>
    /// The EyePosition class is a data-structure for the blend shapes for the eyes
    /// </summary>
    internal class EyePosition {
        public string Category { get; }
        public string BlendShapeName { get; }

        public EyePosition(string category, string blendShapeName) {
            Category = category;
            BlendShapeName = blendShapeName;
        }
    }

    /// <summary>
    /// This class controls the avatars eye position by controlling the corresponding blend shapes.
    /// </summary>
    public class EyeMovementController {
        private readonly BlendShapeController _controller;
        private readonly Dictionary<string, float> _lookValues;
        private readonly Dictionary<string, List<EyePosition>> _eyePositions;

        /// <summary>
        /// The constructor initializes the controllers and parsing of the blend shapes of the avatar.
        /// </summary>
        /// <param name="attachToObject">This game object is the avatar</param>
        public EyeMovementController(GameObject attachToObject) {
            var collectedChildComponents = attachToObject.GetComponentsInChildren<SkinnedMeshRenderer>();

            _controller = new BlendShapeController(collectedChildComponents);

            // This dictionary stores the current emotion values
            _lookValues = new Dictionary<string, float>();

            _eyePositions = ParseEyeBlendShapes();
        }

        /// <summary>
        /// This function parses the blend shapes of the game object and adds them to an internal data-structure
        /// </summary>
        /// <returns>Dictionary of blend shape name as key and list as value</returns>
        private Dictionary<string, List<EyePosition>> ParseEyeBlendShapes() {
            var names = _controller.GetBlendShapeNames();

            var positions = new Dictionary<string, List<EyePosition>>();
            foreach (var name in names) {
                var s = name.Split('-');
                if (!positions.ContainsKey(s[1])) {
                    positions.Add(s[1], new List<EyePosition>());
                }

                if (!_lookValues.ContainsKey(s[1])) {
                    _lookValues.Add(s[1], 0f);
                }
            }

            foreach (var name in names) {
                var s = name.Split('-');

                positions[s[1]].Add(new EyePosition(s[1], name));
            }

            return positions;
        }

        /// <summary>
        /// This function sets the eye position to an x or y value to a weight value. It sets the position gradually
        /// to ensure a smooth transition.
        /// </summary>
        /// <param name="xOrY">A string value which should either by "x" or "y" indicating if the eyes should be moved left or right, or up or down.</param>
        /// <param name="value">The value is a int between -100 and 100. Positive values move the eye right, or the eye up, while negative values move the eye left or down</param>
        /// <param name="time">This parameter represents how long it should take for the eye to move from its current position to the "value" parameter. 1 = 1 second, 0.5 = 500ms</param>
        /// <returns></returns>
        /// <exception cref="Exception">If the "xOrY" parameter isn't "x" or "y" an exception is thrown.</exception>
        public IEnumerator SetEyeXOrYPosition(string xOrY, int value, float time) {
            // Parse the xOrY parameter.
            string pos;
            string posNeg;
            if (xOrY == "x") {
                pos = "right";
                posNeg = "left";
            } else if (xOrY == "y") {
                pos = "up";
                posNeg = "down";
            } else
                throw new Exception("Can only set x or y not " + xOrY + "!");

            // Limit the value to a maximum of 100 or minimum of -100. I.e. a value of 356 is capped at 100.
            var normalized = value;
            if (value > 100)
                normalized = 100;
            if (value < -100)
                normalized = -100;

            var normalizedTime = time;

            // if we wanna look right and we are looking left, first set the left position to 0
            if (normalized > 0) {
                if (_lookValues[posNeg] > 0) {
                    normalizedTime /= 2;
                    yield return SetEyePosition(posNeg, 0, normalizedTime);
                }

                yield return SetEyePosition(pos, normalized, normalizedTime);
            }

            // if we wanna look left and we are looking right, first set the right position to 0
            if (normalized < 0) {
                if (_lookValues[pos] > 0) {
                    normalizedTime /= 2;
                    yield return SetEyePosition(pos, 0, normalizedTime);
                }

                yield return SetEyePosition(posNeg, Math.Abs(normalized), normalizedTime);
            }
        }

        /// <summary>
        /// Set a the eye position using its blend shape name. Here the eye position can be set using position names.
        /// Setting the position "right" to 100 moves the eye position to the right, while setting the position "left"
        /// to 100 moves the eye position to the left. The other two positions are "up" and "down" for vertical eye
        /// positions.
        /// </summary>
        /// <param name="position">Name of the position (right, left, up, down)</param>
        /// <param name="intensity">Value from 0 to 100 indicating where the position is set</param>
        /// <param name="time">The time for the eye position to be set in seconds</param>
        /// <returns></returns>
        public IEnumerator SetEyePosition(string position, float intensity, float time) {
            if (!_eyePositions.ContainsKey(position)) {
                yield return false;
            }

            // We need to keep track of the current intensity and set the new intensity
            var currentIntensity = _lookValues[position];
            var difference = intensity - currentIntensity;
            // Essentially we need to know if we should increment or decrease the current intensity
            var increment = difference > 0;
            difference = Math.Abs(difference);
            _lookValues[position] = intensity;

            // We set the incrementBy value to how big the difference is between the old intensity and the new intensity
            // If this value is greater than 50 we move the eyes quicker.
            var incrementBy = 1;
            if (difference >= 50) incrementBy = 10;

            var waitInterval = time / difference;

            for (var i = 0; i <= difference; i += incrementBy) {
                // Universally set each blend shape with the emotion name to the same intensity

                foreach (var pos in _eyePositions[position]) {
                    if (increment) _controller.SetBlendShape(pos.BlendShapeName, currentIntensity + i);
                    else _controller.SetBlendShape(pos.BlendShapeName, currentIntensity - i);
                }

                yield return new WaitForSeconds(waitInterval);
            }

            foreach (var pos in _eyePositions[position]) {
                _controller.SetBlendShape(pos.BlendShapeName, intensity);
            }

            yield return true;
        }
    }
}

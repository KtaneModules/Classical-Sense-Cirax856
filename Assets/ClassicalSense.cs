using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicalSense : MonoBehaviour
{
#pragma warning disable 0108
    public KMAudio audio;
#pragma warning restore 0108
    public KMBombModule module;

    public Material blackMaterial;

    private KMAudio.KMAudioRef audioRef;

    public KMSelectable leftArrow;
    public KMSelectable rightArrow;
    public KMSelectable playButton;
    public KMSelectable submitButton;

    public GameObject playButtonObject;
    public TextMesh playButtonText;

    public TextMesh pieceText;
    public TextMesh authorText;

    private float resetTimer = 0f;
    private bool alreadyReset = false;

    private bool isPlaying;
    private string playing;

    private bool isSolved = false;

    private Coroutine resetCoroutine;

    private bool timerDone = false;

    // piece audioclips
    [SerializeField] private AudioClip[] Nutcracker;
    [SerializeField] private AudioClip[] Dvorak;
    [SerializeField] private AudioClip[] Mahler;
    [SerializeField] private AudioClip[] Spring;
    [SerializeField] private AudioClip[] Debussy;
    [SerializeField] private AudioClip[] Strauss;
    [SerializeField] private AudioClip[] Requiem;
    [SerializeField] private AudioClip[] Pictures;
    [SerializeField] private AudioClip[] Beethoven;
    [SerializeField] private AudioClip[] Vltava;
    [SerializeField] private AudioClip[] Wagner;
    [SerializeField] private AudioClip[] Bolero;
    [SerializeField] private AudioClip[] Planets;
    [SerializeField] private AudioClip[] Blue;
    [SerializeField] private AudioClip[] Leningrad;

    [SerializeField] private AudioClip clusterChord;
    [SerializeField] private AudioClip solveSound;

    [SerializeField] private Material solveMaterial;

    readonly private string[,] pieces = new string[15, 2]
    {
        { "The Nutcracker Suite", "Pyotr Ilyich\nTchaikovsky" },
        { "Symphony no. 9\n(From the New World)\nmvt. 4", "Antonín Dvořák" },
        { "Symphony no. 2\n(Resurrection) mvt. 5", "Gustav Mahler" },
        { "The Rite of Spring", "Igor Stavinsky" },
        { "Prelud to the Afternoon\nof a Faun", "Claude Debussy" },
        { "Der Rosenkavalier Suite", "Richard Strauss" },
        { "Requiem in D minor", "Wolgang Amadeus\nMozart" },
        { "Pictures at an\nExhibition", "Modest Petrovich\nMussorgsky" },
        { "Symphony no. 9 mvt. 4", "Ludwig van Beethoven" },
        { "Vltava", "Bedřich Smetana" },
        { "Siegfried Idyll", "Richard Wagner" },
        { "Bolero", "Maurice Ravel" },
        { "The Planets", "Gustav Holst" },
        { "Rhapsody in Blue", "George Gershwin" },
        { "Symphony no 7.\n(Leningrad) mvt. 4", "Dmitri Dmitriyevich\nShostakovich" }
   };

    readonly private string[,] logPieces = new string[15, 2]
    {
        { "The Nutcracker Suite", "Pyotr Ilyich Tchaikovsky" },
        { "Symphony no. 9 (From the New World) mvt. 4", "Antonín Dvořák" },
        { "Symphony no. 2 (Resurrection) mvt. 5", "Gustav Mahler" },
        { "The Rite of Spring", "Igor Stavinsky" },
        { "Prelud to the Afternoon of a Faun", "Claude Debussy" },
        { "Der Rosenkavalier Suite", "Richard Strauss" },
        { "Requiem in D minor", "Wolgang Amadeus Mozart" },
        { "Pictures at an Exhibition", "Modest Petrovich\nMussorgsky" },
        { "Symphony no. 9 mvt. 4", "Ludwig van Beethoven" },
        { "Vltava", "Bedřich Smetana" },
        { "Siegfried Idyll", "Richard Wagner" },
        { "Bolero", "Maurice Ravel" },
        { "The Planets", "Gustav Holst" },
        { "Rhapsody in Blue", "George Gershwin" },
        { "Symphony no 7 (Leningrad) mvt. 4", "Dmitri Dmitriyevich Shostakovich" }
    };

    readonly private string[][] snippets = new string[][]
    {
        new string[] {"Nutcracker1", "Nutcracker2", "Nutcracker3", "Nutcracker4", "Nutcracker5", "Nutcracker6", "Nutcracker7", "Nutcracker8", "Nutcracker9"}, // The Nutcracker Suite
        new string[] {"Dvorak1", "Dvorak2", "Dvorak3", "Dvorak4", "Dvorak5", "Dvorak6"}, // Symphony no. 9 (From the New World) mvt. 4
        new string[] {"Mahler1", "Mahler2", "Mahler3", "Mahler4", "Mahler5", "Mahler6", "Mahler7", "Mahler8", "Mahler9", "Mahler10", "Mahler11", "Mahler12", "Mahler13", "Mahler14", "Mahler15"}, // Symphony no. 2 (Ressurection) mvt. 5
        new string[] {"Spring1", "Spring2", "Spring3", "Spring4", "Spring5", "Spring6", "Spring7", "Spring8", "Spring9", "Spring10"}, // The Rite of Spring
        new string[] {"Debussy1", "Debussy2", "Debussy3", "Debussy4", "Debussy5", "Debussy6"}, // Prelud to the Afternoon of a Faun
        new string[] {"Strauss1", "Strauss2", "Strauss3", "Strauss4", "Strauss5", "Strauss6", "Strauss7", "Strauss8", "Strauss9", "Strauss10", "Strauss11", "Strauss12"}, // Der Rosenkavalier Suite
        new string[] {"Requiem1", "Requiem2", "Requiem3", "Requiem4", "Requiem5", "Requiem6", "Requiem7", "Requiem8", "Requiem9", "Requiem10", }, // Requiem in D minor
        new string[] {"Pictures1", "Pictures2", "Pictures3", "Pictures4", "Pictures5", "Pictures6", "Pictures7", "Pictures8", "Pictures9", "Pictures10"}, // Pictures at an Exhibition
        new string[] {"Beethoven1", "Beethoven2", "Beethoven3", "Beethoven4", "Beethoven5", "Beethoven6", "Beethoven7", "Beethoven8"}, // Symphony no. 9 mvt. 4
        new string[] {"Vltava1", "Vltava2", "Vltava3", "Vltava4", "Vltava5", "Vltava6", "Vltava7"}, // Vltava
        new string[] {"Wagner1", "Wagner2", "Wagner3", "Wagner4", "Wagner5", "Wagner6"}, // Siegfried Idyll
        new string[] {"Bolero1", "Bolero2", "Bolero3", "Bolero4", "Bolero5", "Bolero6"}, // Bolero
        new string[] {"Planets1", "Planets2", "Planets3", "Planets4", "Planets5", "Planets6", "Planets7", "Planets8", "Planets9", "Planets10", "Planets11"}, // The Planets
        new string[] {"Blue1", "Blue2", "Blue3", "Blue4", "Blue5", "Blue6", "Blue7"}, // Rhapsody in Blue
        new string[] {"Leningrad1", "Leningrad2", "Leningrad3", "Leningrad4", "Leningrad5", "Leningrad6", "Leningrad7", "Leningrad8"}, // Symphony no 7. (Leningrad) mvt. 4
    };

    private AudioClip[][] clips;

    private int[] randomPlays = new int[2] {0, 0};
    private int playIndex = 1;

    private int index = 0;

    private int solutionIndex;

    static int moduleIdCount = 1;
#pragma warning disable 0414
    int moduleId;
#pragma warning restore 0414

    private void Awake()
        => solveMaterial.color = new Color(0f, 135f/255f, 68f/255f);

    private void Start()
    {
        clips = new AudioClip[][]
        {
            Nutcracker,
            Dvorak,
            Mahler,
            Spring,
            Debussy,
            Strauss,
            Requiem,
            Pictures,
            Beethoven,
            Vltava,
            Wagner,
            Bolero,
            Planets,
            Blue,
            Leningrad
        };

        leftArrow.OnInteract += delegate () { left(); return false; };
        rightArrow.OnInteract += delegate () { right(); return false; };
        playButton.OnInteract += delegate () { playPress(); return false; };
        submitButton.OnInteract += delegate () { submit(); return false; };

        playButton.OnInteractEnded += delegate () { resetHandler(); };

        moduleId = moduleIdCount++;

        isSolved = false;

        // display text
        pieceText.text = pieces[index, 0];
        authorText.text = pieces[index, 1];

        solutionIndex = Random.Range(0, 14);

        Debug.LogFormat("[Classical Sense #{0}] Generated piece: {1} by {2}.", moduleId, logPieces[solutionIndex, 0], logPieces[solutionIndex, 1]);

        randomPlays[0] = Random.Range(0, snippets[solutionIndex].Length - 1);
        randomPlays[1] = Random.Range(0, snippets[solutionIndex].Length - 1);
        while (randomPlays[0] == randomPlays[1])
        {
            randomPlays[1] = Random.Range(0, snippets[solutionIndex].Length - 1);
        }
    }

    void left()
    {
        if(!isSolved)
        {
            index--;
            if (index < 0)
            {
                index = 14;
            }

            pieceText.text = pieces[index, 0];
            authorText.text = pieces[index, 1];
        }
    }

    void right()
    {
        if(!isSolved)
        {
            index++;
            if (index > 14)
                index = 0;

            pieceText.text = pieces[index, 0];
            authorText.text = pieces[index, 1];
        }
    }

    private Coroutine soundCoroutine;
#pragma warning disable 0414
    private Coroutine clusterCoroutine;
#pragma warning restore 0414
    private Coroutine playAnimCoroutine;
    private Coroutine submitCoroutine;

    void play()
    {
        if(!isSolved)
        {
            audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);

            if (playAnimCoroutine != null)
                StopCoroutine(playAnimCoroutine);
            playAnimCoroutine = StartCoroutine(playAnim());

            if (isPlaying)
            {
                if (playing == "snippet") StopCoroutine(soundCoroutine);
                if (playing == "cluster") StopCoroutine(clusterCoroutine);
                if (playing == "solve") StopCoroutine(solveCoroutine);

                audioRef?.StopSound();
                isPlaying = false;

                playButtonText.text = "►";
                playButtonText.transform.localPosition = new Vector3(0.057f, 1.12f, 0.012f);
            }
            else
            {
                if (soundCoroutine != null) StopCoroutine(soundCoroutine);

                soundCoroutine = StartCoroutine(PlaySnippet(clips[solutionIndex][randomPlays[playIndex]]));

                if (playIndex == 0)
                {
                    playIndex = 1;
                }
                else
                {
                    playIndex = 0;
                }

                playButtonText.text = "■";
                playButtonText.transform.localPosition = new Vector3(0f, 1.12f, 0f);
            }
        }
    }

    private IEnumerator playAnim(float duration = .075f)
    {
        float timer = 0f;
        while (timer < duration)
        {
            yield return null;
            timer += Time.deltaTime;

            playButton.transform.localPosition = Vector3.Lerp(new Vector3(-0.0532f, 0.014f, 0.0588f), new Vector3(-0.0532f, 0.004f, 0.0588f), timer / duration);
        }

        timer = 0f;
        while (timer < duration)
        {
            yield return null;
            timer += Time.deltaTime;

            playButton.transform.localPosition = Vector3.Lerp(new Vector3(-0.0532f, 0.004f, 0.0588f), new Vector3(-0.0532f, 0.014f, 0.0588f), timer / duration);
        }

        playButton.transform.localPosition = new Vector3(-0.0532f, 0.014f, 0.0588f);
    }

    private IEnumerator submitAnim(float duration = .075f)
    {
        float timer = 0f;
        while (timer < duration)
        {
            yield return null;
            timer += Time.deltaTime;

            submitButton.transform.localPosition = Vector3.Lerp(new Vector3(0.0071f, 0.014f, 0.0588f), new Vector3(0.0071f, 0.004f, 0.0588f), timer / duration);
        }

        timer = 0f;
        while (timer < duration)
        {
            yield return null;
            timer += Time.deltaTime;

            submitButton.transform.localPosition = Vector3.Lerp(new Vector3(0.0071f, 0.004f, 0.0588f), new Vector3(0.0071f, 0.014f, 0.0588f), timer / duration);
        }

        submitButton.transform.localPosition = new Vector3(0.0071f, 0.014f, 0.0588f);
    }

    private IEnumerator submitColorAnim(float duration = .75f)
    {
        Color targetColor = new Color(Random.value, Random.value, Random.value);

        float timer = 0f;
        while (timer < duration)
        {
            yield return null;
            timer += Time.deltaTime;

            solveMaterial.color = Color.Lerp(solveMaterial.color, targetColor, timer / duration);
        }

        solveMaterial.color = targetColor;
    }

    private IEnumerator PlaySnippet(AudioClip sound)
    {
        audioRef?.StopSound();
        isPlaying = true;
        playing = "snippet";

        audioRef = audio.PlaySoundAtTransformWithRef(sound.name, transform);
        yield return new WaitForSeconds(sound.length);
        isPlaying = false;
        audioRef?.StopSound();
    }

#pragma warning disable 0414
    private Coroutine solveCoroutine;
#pragma warning restore 0414

    void submit()
    {
        if(!isSolved)
        {
            if (submitCoroutine != null)
                StopCoroutine(submitCoroutine);
            submitCoroutine = StartCoroutine(submitAnim());

            if (index == solutionIndex)
            {
                isSolved = true;

                audioRef?.StopSound();

                if (soundCoroutine != null) StopCoroutine(soundCoroutine);
                solveCoroutine = StartCoroutine(playSolveSound());

                StartCoroutine(submitColorAnim());

                Debug.LogFormat("[Classical Sense #{0}] Successfully submit the correct piece ({1} by {2}). Module solved.", moduleId, logPieces[solutionIndex, 0], logPieces[solutionIndex, 1]);
                module.HandlePass();
            }
            else
            {
                audioRef?.StopSound();

                Debug.LogFormat("[Classical Sense #{0}] Strike! Submited {1} by {2}, expected {3} by {4}.", moduleId, logPieces[index, 0], logPieces[index, 1], logPieces[solutionIndex, 0], logPieces[solutionIndex, 1]);
                module.HandleStrike();
            }
        }
    }

    private IEnumerator playSolveSound()
    {
        audioRef?.StopSound();
        isPlaying = true;
        playing = "solve";

        audioRef = audio.PlaySoundAtTransformWithRef(solveSound.name, transform);
        yield return new WaitForSeconds(solveSound.length);
        audioRef?.StopSound();
        isPlaying = false;
    }

    private IEnumerator trackTimer()
    {
        while (!timerDone)
        {
            yield return null;
            resetTimer += Time.deltaTime;
            if (resetTimer >= 5f)
            {
                audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
                timerDone = true;
            }
        }
    }

    void playPress()
    {
        if(!alreadyReset && !isSolved)
            resetCoroutine = StartCoroutine(trackTimer());
    }

    void resetHandler()
    {
        if(!isSolved)
        {
            if(resetCoroutine != null) StopCoroutine(resetCoroutine);

            if (resetTimer < 5f)
                play();
            else
                reset();

            resetTimer = 0f;
            timerDone = false;
        }
    }

    void reset()
    {
        audioRef?.StopSound();

        if (soundCoroutine != null) StopCoroutine(soundCoroutine);
        clusterCoroutine = StartCoroutine(playCluster());

        alreadyReset = true;

        solutionIndex = Random.Range(0, 14);
        index = 0;

        pieceText.text = pieces[index, 0];
        authorText.text = pieces[index, 1];

        randomPlays[0] = Random.Range(0, snippets[solutionIndex].Length - 1);
        randomPlays[1] = Random.Range(0, snippets[solutionIndex].Length - 1);
        while (randomPlays[0] == randomPlays[1])
            randomPlays[1] = Random.Range(0, snippets[solutionIndex].Length - 1);

        playButtonObject.GetComponent<MeshRenderer>().material = blackMaterial;
        playButtonText.color = Color.white;

        Debug.LogFormat("[Classical Sense #{0}] Regenerated piece: {1} by {2}.", moduleId, logPieces[solutionIndex, 0], logPieces[solutionIndex, 1]);
    }

    private IEnumerator playCluster()
    {
        audioRef?.StopSound();
        isPlaying = true;
        playing = "cluster";

        audioRef = audio.PlaySoundAtTransformWithRef(clusterChord.name, transform);
        yield return new WaitForSeconds(clusterChord.length);
        audioRef?.StopSound();
        isPlaying = false;
    }

    // twitch plays
#pragma warning disable 0414
    readonly private string TwitchHelpMessage = "Listen to the snippet using \"!{0} play\", cycle through the pieces using \"!{0} left [amount]\" and \"{0} right [amount]\" (deafult is 1 without the argument). Submit your answer using \"!{0} submit\". You can reset the module using \"!{0} reset\".";
#pragma warning restore 0414

    private IEnumerator ProcessTwitchCommand(string command)
    {
        command = command.ToLowerInvariant();

        if(command.StartsWith("play") || command.StartsWith("listen"))
        {
            yield return null;
            playButton.OnInteract();
            yield return new WaitForSeconds(0.02f);
            playButton.OnInteractEnded();
        } else if(command.StartsWith("left"))
        {
            string editedLeft = command.Remove(0, 4);
            int amount = 1;
            if((editedLeft.Length == 2) && (!(char.IsDigit(editedLeft[1])) || (editedLeft[1] - '0' < 0)))
            {
                yield return "sendtocharerror!h Please input how many steps you want to move left. Input no argument if you want to go by 1.";
            } else if(editedLeft.Length == 2)
            {
                amount = editedLeft[1] - '0';
            }
            yield return null;
            for(int i = 0; i < amount; i++)
            {
                yield return null;
                yield return new WaitForSeconds(0.2f);
                leftArrow.OnInteract();
            }
        } else if(command.StartsWith("right"))
        {
            string editedRight = command.Remove(0, 5);
            int amount = 1;
            if ((editedRight.Length == 2) && (!(char.IsDigit(editedRight[1])) || (editedRight[1] - '0' < 0)))
            {
                yield return "sendtocharerror!h Please input how many steps you want to move right. Input no argument if you want to go by 1.";
            }
            else if (editedRight.Length == 2)
            {
                amount = editedRight[1] - '0';
            }
            yield return null;
            for (int i = 0; i < amount; i++)
            {
                yield return null;
                rightArrow.OnInteract();
            }
        } else if(command.StartsWith("submit"))
        {
            yield return null;
            submit();
        } else if(command.StartsWith("reset") || command.StartsWith("restart"))
        {
            yield return null;
            playButton.OnInteract();
            yield return new WaitForSeconds(5.2f);
            playButton.OnInteractEnded();
        }
    }

    private IEnumerator TwitchHandleForcedSolve()
    {
        int difference;

        if(index > solutionIndex)
        {
            difference = index - solutionIndex;
            for(int i = 0; i < difference; i++)
            {
                yield return null;
                leftArrow.OnInteract();
                yield return new WaitForSeconds(0.05f);
            }
        }
        else if(index < solutionIndex)
        {
            difference = solutionIndex - index;
            for(int i = 0; i < difference; i++)
            {
                yield return null;
                rightArrow.OnInteract();
                yield return new WaitForSeconds(0.05f);
            }
        }

        yield return new WaitForSeconds(0.2f);

        yield return null;
        submitButton.OnInteract();
    }
}

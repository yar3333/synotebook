using System.Collections.Generic;
using System.Threading;

namespace SyNotebook;

public static class NotesAsyncLoader
{
    private static readonly List<Note> notesToLoad = new();
    private static Thread thread;

	public static void Add(Note note)
	{
		notesToLoad.Add(note);
	}

	public static void Start()
	{
		thread = new Thread(Loading);
		thread.Start();
	}

    private static bool needToStop;
	public static void Abort()
	{
		needToStop = true;
		while (thread != null && thread.IsAlive) Thread.Sleep(20);
	}

    private static void Loading()
	{
		while (!needToStop && notesToLoad.Count>0)
		{
			var note = notesToLoad[0];
			note.ContinueLoading();
			notesToLoad.RemoveAt(0);
		}
		
		if (needToStop)
		{
			foreach (var note in notesToLoad) note.AbortLoading();
		}
	}
}

using easysave;

Save save = new Save(DateTime.Now, "Sauvegarde de test", "Ceci est une sauvegarde de test pour tester les tests unitaires.");
Console.WriteLine(save.displaySaveInformation());
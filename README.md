## UML Use Case for persistent data
image here

## UML Class Diagram for persistent data
image here

## LevelData class pseudo code
```
  class LevelData
    LevelData leveldata
    int numberOfLevels
    List<Level> myLevels

    OnStart(){
      if levelData == null
      {
        levelData = this
        ReadList()
      }else{
        Destroy(this)
      }
    }
    SaveList(){
      ConnectToFile('save.dat')
      Serialize(myLevels)
      CloseConnection()
    }
    ReadList(){
      if save.dat exists
      {
        myLevels = Deserialize("save.dat")
      }else{
        PopulateList()
      }
    }
    PopulateList(){
      for numberOfLevels
      {
        myLevels.Add(newLevel)
      }
      UnlockFirstLevel()
      SaveList()
    }
```

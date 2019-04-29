## UML Use Case for persistent data
![](https://github.com/HoaxShark/comp260-server/blob/master/Design%20Documents/Client%20Class%20UML.png?raw=true)  

## UML Class Diagram for persistent data
![](https://github.com/HoaxShark/comp260-server/blob/master/Design%20Documents/Client%20Class%20UML.png?raw=true)  

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

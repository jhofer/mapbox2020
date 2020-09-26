using Endgame.DTOs;

public class BuildingApi : BaseSingleton<BuildingApi>
{



    // Start is called before the first frame update
    void Start()
    {
        Hub.Instance.On<BuildingDto>("UpdateBuilding", UpdateBuilding);
    }

    private void UpdateBuilding(BuildingDto obj)
    {


        var buildings = MapUtils.Instance.map.GetComponentsInChildren<BuildingContoller>();
        foreach (var b in buildings)
        {
            if (b.MapBoxId == obj.id)
            {
                b.UpdateBuilding(obj);
                return;
            }
        }
    }

    public async void ConquerBuilding(BuildingDto building)
    {
        await Hub.Instance.PostAsync<object, BuildingDto>("/api/buildings", building);
               
    }


}
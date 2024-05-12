using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.Metrics;
using System.Text.Json;
using TaskGmbhRestaurantTracker.DataObjects;
using System.Collections.Generic;


namespace TaskGmbhRestaurantTracker.Pages.Home
   {
    
    public class HomeBase : ComponentBase
    {
        
       
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        public List<DataObject1> dataList = new List<DataObject1>();
        public int CountId { get; set; }
        protected override async void OnInitialized()
        {
            CountId = 0;
            await LoadItems();

            

            StateHasChanged();
        }
        private async Task LoadItems()
		{
			var serializedData = await JSRuntime.InvokeAsync<string>( "localStorage.getItem", "dynamicListItems" );
			if( !string.IsNullOrEmpty( serializedData ) )
			{
				dataList = JsonSerializer.Deserialize<List<DataObject1>>( serializedData);
				CountId = dataList.Count;
			}
		}
        public async Task AddItem(string name, string note, string url)
        {
            CountId++;
            DataObject1 data = new DataObject1()
            {
                Id = CountId,
                Restaurant = name,
                Note = note,
                Url = url,
                Like = 0,
                Dislike = 0,

            };

            dataList.Add(data);
            Console.WriteLine(dataList);
            var serializedData = JsonSerializer.Serialize(dataList);
            await JSRuntime.InvokeAsync<object>("localStorage.setItem", "dynamicListItems", serializedData);


        }
    }
}

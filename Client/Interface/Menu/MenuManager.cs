﻿using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Client.Handlers;
using IgiCore.Client.Managers;
using NativeUI;

namespace IgiCore.Client.Interface.Menu
{
	public class MenuManager : Manager
	{
		protected readonly MenuPool Pool = new MenuPool();

		public CharacterMenu CharacterMenu => new CharacterMenu();

		public MenuManager()
		{
			TickHandler.Attach<MenuManager>(this.Render);

			Client.Instance.OnCharacterLoaded += (sender, args) =>
			{
				this.CharacterMenu.WalkingStyleMenu.Index = this.CharacterMenu.WalkingStyles.FindIndex(s => s.Value == Client.Instance.User?.Character?.WalkingStyle);
			};

			this.Pool.Add(this.CharacterMenu);
			this.Pool.RefreshIndex();
		}

		public async Task Render()
		{
			this.Pool.ProcessMenus();

			if (Input.Input.IsControlJustReleased(Control.MultiplayerInfo)) // Z
			{
				this.CharacterMenu.Visible = !this.CharacterMenu.Visible;
			}
		}

		public override void Dispose()
		{
			TickHandler.Dettach<MenuManager>();
		}
	}
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;

public class UCardGame : AHotBase
{
	Button btnReturn;
	Text textMyLevel;
	Text textMyAvatarname;
	Text textMyCardCount;
	Text textOtherLevel;
	Text textOtherAvatarname;
	Text textOtherCardCount;
	Button btnJoinRoom;

	Transform cardcell;
	RawImage mycard;
	protected override void InitComponents()
	{
		mycard = FindWidget<RawImage>("mycard");
		var bgpath = "Images/Pai/bg1";
		UHotAssetBundleLoader.Instance.OnDownloadResources(() =>
		{
			mycard.texture = UHotAssetBundleLoader.Instance.OnLoadAsset<Texture2D>(bgpath);
		}, bgpath);

		textMyCardCount = FindWidget<Text>("textMyCardCount");
		textMyCardCount.text = "0";

		textMyAvatarname = FindWidget<Text>("textMyAvatarname");
		textMyAvatarname.text = URemoteData.AvatarName;

		textMyLevel = FindWidget<Text>("textMyLevel");
		ShowLevel();

		textOtherCardCount = FindWidget<Text>("textOtherCardCount");
		textOtherCardCount.text = "0";

		textOtherLevel = FindWidget<Text>("textOtherLevel");
		textOtherAvatarname = FindWidget<Text>("textOtherAvatarname");

		btnReturn = FindWidget<Button>("btnReturn");
		btnReturn.onClick.AddListener(() =>
		{
			OnUnloadThis();

			LoadAnotherUI<UIMain>();
		});

		btnJoinRoom = FindWidget<Button>("btnJoinRoom");
		btnJoinRoom.onClick.AddListener(() =>
		{
			WebSocketConnector.Instance.OnRemoteCall("joinRoom", "", OnJoinRoomCB);
		});

		cardcell = FindWidget<Transform>("cardcell");
		cardcell.gameObject.SetActive(false);

		for (var i = 0; i < 10; i++)
		{
			var obj = GameObject.Instantiate(cardcell, cardcell.parent);
			obj.gameObject.SetActive(true);
			var rawimage = obj.GetComponent<RawImage>();
			var texturePath = $"Images/Pai/b{i + 1}";
			UHotAssetBundleLoader.Instance.OnDownloadResources(() =>
			{
				rawimage.texture = UHotAssetBundleLoader.Instance.OnLoadAsset<Texture2D>(texturePath);
			}, texturePath);
		}

		ShowWidget("otherinfo", false);
		URemoteData.ListeningParam(InfoNameDefs.AvatarLevel, ShowLevel);

		UICommonWait.Show();
		WebSocketConnector.Instance.OnInit(Utils.WebSocketURL + UILogin.CachedUsernameAndTokenArguments, evt =>
		{
			UICommonWait.Hide();

		}, msgEvt =>
		{
		}, errEvt =>
		{
			UICommonWait.Hide();
		}, closeEvt =>
		{
			UICommonWait.Hide();
		});
	}

	private void OnJoinRoomCB(string obj)
	{
		btnJoinRoom.gameObject.SetActive(false);
	}

	protected override void OnDestroy()
	{
		URemoteData.CancelListeningParam(InfoNameDefs.AvatarLevel, ShowLevel);
	}

	private void ShowLevel()
	{
		textMyLevel.text = URemoteData.AvatarLevel;
	}
}

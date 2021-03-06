resource_manifest_version '05cfa83c-a124-4cfa-a768-c24a5811d8f9'
description 'IgiCore Framework'
version '0.0.1'

server_scripts {
	'Core.net.dll',
	'Server.net.dll',
}

client_scripts {
	'Core.net.dll',
	'Client.net.dll',
}

files {
	'System.ComponentModel.DataAnnotations.dll',
	'Newtonsoft.Json.dll',
	'EntityFramework.dll',
	'NativeUI.dll',

	'Interface/loading-screen/index.html',
	'Interface/loading-screen/main.js',
	'Interface/loading-screen/main.css',
	'Interface/loading-screen/libs/jquery.min.js',
	'Interface/loading-screen/libs/bootstrap.min.css',
	'Interface/loading-screen/img/logo.png',
	'Interface/loading-screen/img/bg1.jpg',
	'Interface/loading-screen/img/bg2.jpg',
	'Interface/loading-screen/img/bg3.jpg',
	'Interface/loading-screen/img/bg4.jpg',
	'Interface/loading-screen/img/bg5.jpg',

	'Interface/dist/index.html',
	'Interface/dist/bundle.js',
	'Interface/dist/gravity.woff2',
	'Interface/dist/pricedown.woff2',
	'Interface/dist/male.png',
}

loadscreen 'Interface/loading-screen/index.html'
ui_page 'Interface/dist/index.html'

-- Resource Metadata
fx_version 'cerulean'
games { 'rdr3', 'gta5' }

author 'Apache Scripting'
description 'An AI Paramedic help you if you are dead'
version '1.0.0'

-- What to run
files {
	'Config/*.json',
	'Newtonsoft.Json.dll'
}

server_script 'ApacheIAMedicServer.net.dll'
client_script 'ApacheIAMedicClient.net.dll'
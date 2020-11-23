<?php
	require_once("config.php");

	$auth_host = $GLOBALS['auth_host'];
	$auth_user = $GLOBALS['auth_user'];
	$auth_pass = $GLOBALS['auth_pass'];
	$auth_dbase = $GLOBALS['auth_dbase'];

	$db = mysqli_connect($auth_host, $auth_user, $auth_pass,$auth_dbase) or die("Error " . mysqli_error($db));
	
	$key = mysqli_real_escape_string($db,$_POST['key']);
	$value = mysqli_real_escape_string($db,$_POST['value']);

	mysqli_query($db,"DELETE FROM ints WHERE name = '$key'");
	mysqli_query($db,"INSERT INTO ints(name,value) VALUES ('$key','$value')");
	mysqli_close($db);
?> 
 
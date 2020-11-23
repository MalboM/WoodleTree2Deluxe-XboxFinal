<?php
	require_once("config.php");

	$auth_host = $GLOBALS['auth_host'];
	$auth_user = $GLOBALS['auth_user'];
	$auth_pass = $GLOBALS['auth_pass'];
	$auth_dbase = $GLOBALS['auth_dbase'];

	$db = mysqli_connect($auth_host, $auth_user, $auth_pass,$auth_dbase) or die("Error " . mysqli_error($db));
	
	$key = mysqli_real_escape_string($db,$_POST['key']);

	$x = mysqli_real_escape_string($db,$_POST['x']);
	$y = mysqli_real_escape_string($db,$_POST['y']);
	$z = mysqli_real_escape_string($db,$_POST['z']);

	mysqli_query($db,"DELETE FROM vectors WHERE name = '$key'");
	mysqli_query($db,"INSERT INTO vectors(name,x,y,z) VALUES ('$key','$x','$y','$z')");
	mysqli_close($db);
?> 
 
<?php
	require_once("config.php");

	$auth_host = $GLOBALS['auth_host'];
	$auth_user = $GLOBALS['auth_user'];
	$auth_pass = $GLOBALS['auth_pass'];
	$auth_dbase = $GLOBALS['auth_dbase'];

	$db = mysqli_connect($auth_host, $auth_user, $auth_pass,$auth_dbase) or die("Error " . mysqli_error($db));
	
	$key = mysqli_real_escape_string($db,$_POST['key']);

	mysqli_query("DELETE FROM strings WHERE name = '$key'");
	mysqli_query("DELETE FROM ints WHERE name = '$key'");
	mysqli_query("DELETE FROM bools WHERE name = '$key'");
	mysqli_query("DELETE FROM vectors WHERE name = '$key'");
	mysqli_query("DELETE FROM floats WHERE name = '$key'");

	mysqli_close($db);
?> 
 
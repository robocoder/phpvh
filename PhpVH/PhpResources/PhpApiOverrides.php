/*
function is_array_override($var)
{
	return (is_array($var) || $var instanceof ArrayAccess);
}

function array_key_exists_override($key, $search)
{
	if ($search instanceof SuperGlobalOverride)
		return $search->offsetExists($key);		
	else
		return array_key_exists($key, $search);		
}

function extract_override($key, $search)
{
	if ($search instanceof SuperGlobalOverride)
		return $search->offsetExists($key);		
	else
		return array_key_exists($key, $search);		
}
*/



function is_array_override($param0)
{
	$var0;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;

	if ($numArgs == 0) $ret = is_array();
	else if ($numArgs == 1) $ret = is_array($var0);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;

	
	return $ret;
}

function array_change_key_case_override($param0,$param1=null)
{
	$var0;
	$var1;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;

	if ($numArgs == 0) $ret = array_change_key_case();
	else if ($numArgs == 1) $ret = array_change_key_case($var0);
	else if ($numArgs == 2) $ret = array_change_key_case($var0,$var1);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;

	
	return $ret;
}

function array_chunk_override($param0,$param1,$param2=null)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = array_chunk();
	else if ($numArgs == 1) $ret = array_chunk($var0);
	else if ($numArgs == 2) $ret = array_chunk($var0,$var1);
	else if ($numArgs == 3) $ret = array_chunk($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function array_combine_override($param0,$param1)
{
	$var0;
	$var1;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;

	if ($numArgs == 0) $ret = array_combine();
	else if ($numArgs == 1) $ret = array_combine($var0);
	else if ($numArgs == 2) $ret = array_combine($var0,$var1);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;

	
	return $ret;
}

function array_count_values_override($param0)
{
	$var0;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;

	if ($numArgs == 0) $ret = array_count_values();
	else if ($numArgs == 1) $ret = array_count_values($var0);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;

	
	return $ret;
}

function array_diff_assoc_override($param0,$param1,$param2=null)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = array_diff_assoc();
	else if ($numArgs == 1) $ret = array_diff_assoc($var0);
	else if ($numArgs == 2) $ret = array_diff_assoc($var0,$var1);
	else if ($numArgs == 3) $ret = array_diff_assoc($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function array_diff_key_override($param0,$param1,$param2=null)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = array_diff_key();
	else if ($numArgs == 1) $ret = array_diff_key($var0);
	else if ($numArgs == 2) $ret = array_diff_key($var0,$var1);
	else if ($numArgs == 3) $ret = array_diff_key($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function array_diff_uassoc_override($param0,$param1,$param2)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = array_diff_uassoc();
	else if ($numArgs == 1) $ret = array_diff_uassoc($var0);
	else if ($numArgs == 2) $ret = array_diff_uassoc($var0,$var1);
	else if ($numArgs == 3) $ret = array_diff_uassoc($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function array_diff_ukey_override($param0,$param1,$param2)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = array_diff_ukey();
	else if ($numArgs == 1) $ret = array_diff_ukey($var0);
	else if ($numArgs == 2) $ret = array_diff_ukey($var0,$var1);
	else if ($numArgs == 3) $ret = array_diff_ukey($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function array_diff_override($param0,$param1,$param2=null)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = array_diff();
	else if ($numArgs == 1) $ret = array_diff($var0);
	else if ($numArgs == 2) $ret = array_diff($var0,$var1);
	else if ($numArgs == 3) $ret = array_diff($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function array_fill_keys_override($param0,$param1)
{
	$var0;
	$var1;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;

	if ($numArgs == 0) $ret = array_fill_keys();
	else if ($numArgs == 1) $ret = array_fill_keys($var0);
	else if ($numArgs == 2) $ret = array_fill_keys($var0,$var1);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;

	
	return $ret;
}

function array_fill_override($param0,$param1,$param2)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = array_fill();
	else if ($numArgs == 1) $ret = array_fill($var0);
	else if ($numArgs == 2) $ret = array_fill($var0,$var1);
	else if ($numArgs == 3) $ret = array_fill($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function array_filter_override($param0,$param1=null)
{
	$var0;
	$var1;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;

	if ($numArgs == 0) $ret = array_filter();
	else if ($numArgs == 1) $ret = array_filter($var0);
	else if ($numArgs == 2) $ret = array_filter($var0,$var1);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;

	
	return $ret;
}

function array_flip_override($param0)
{
	$var0;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;

	if ($numArgs == 0) $ret = array_flip();
	else if ($numArgs == 1) $ret = array_flip($var0);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;

	
	return $ret;
}

function array_intersect_assoc_override($param0,$param1,$param2=null)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = array_intersect_assoc();
	else if ($numArgs == 1) $ret = array_intersect_assoc($var0);
	else if ($numArgs == 2) $ret = array_intersect_assoc($var0,$var1);
	else if ($numArgs == 3) $ret = array_intersect_assoc($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function array_intersect_key_override($param0,$param1,$param2=null)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = array_intersect_key();
	else if ($numArgs == 1) $ret = array_intersect_key($var0);
	else if ($numArgs == 2) $ret = array_intersect_key($var0,$var1);
	else if ($numArgs == 3) $ret = array_intersect_key($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function array_intersect_uassoc_override($param0,$param1,$param2)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = array_intersect_uassoc();
	else if ($numArgs == 1) $ret = array_intersect_uassoc($var0);
	else if ($numArgs == 2) $ret = array_intersect_uassoc($var0,$var1);
	else if ($numArgs == 3) $ret = array_intersect_uassoc($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function array_intersect_ukey_override($param0,$param1,$param2)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = array_intersect_ukey();
	else if ($numArgs == 1) $ret = array_intersect_ukey($var0);
	else if ($numArgs == 2) $ret = array_intersect_ukey($var0,$var1);
	else if ($numArgs == 3) $ret = array_intersect_ukey($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function array_intersect_override($param0,$param1,$param2=null)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = array_intersect();
	else if ($numArgs == 1) $ret = array_intersect($var0);
	else if ($numArgs == 2) $ret = array_intersect($var0,$var1);
	else if ($numArgs == 3) $ret = array_intersect($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function array_key_exists_override($param0,$param1)
{
	$var0;
	$var1;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;

	if ($numArgs == 0) $ret = array_key_exists();
	else if ($numArgs == 1) $ret = array_key_exists($var0);
	else if ($numArgs == 2) $ret = array_key_exists($var0,$var1);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;

	
	return $ret;
}

function array_keys_override($param0,$param1=null,$param2=null)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = array_keys();
	else if ($numArgs == 1) $ret = array_keys($var0);
	else if ($numArgs == 2) $ret = array_keys($var0,$var1);
	else if ($numArgs == 3) $ret = array_keys($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function array_map_override($param0,$param1,$param2=null)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = array_map();
	else if ($numArgs == 1) $ret = array_map($var0);
	else if ($numArgs == 2) $ret = array_map($var0,$var1);
	else if ($numArgs == 3) $ret = array_map($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function array_merge_recursive_override($param0,$param1,$param2=null)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = array_merge_recursive();
	else if ($numArgs == 1) $ret = array_merge_recursive($var0);
	else if ($numArgs == 2) $ret = array_merge_recursive($var0,$var1);
	else if ($numArgs == 3) $ret = array_merge_recursive($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function array_merge_override($param0,$param1,$param2=null)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = array_merge();
	else if ($numArgs == 1) $ret = array_merge($var0);
	else if ($numArgs == 2) $ret = array_merge($var0,$var1);
	else if ($numArgs == 3) $ret = array_merge($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function array_multisort_override(&$param0,$param1=null,$param2=null,$param3=null,$param4=null,$param5=null)
{
	$var0;
	$var1;
	$var2;
	$var3;
	$var4;
	$var5;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;
	if ($numArgs >= 4)
		$var3 = $param3 instanceof SuperGlobalOverride ? $param3->container : $param3;
	if ($numArgs >= 5)
		$var4 = $param4 instanceof SuperGlobalOverride ? $param4->container : $param4;
	if ($numArgs >= 6)
		$var5 = $param5 instanceof SuperGlobalOverride ? $param5->container : $param5;

	if ($numArgs == 0) $ret = array_multisort();
	else if ($numArgs == 1) $ret = array_multisort($var0);
	else if ($numArgs == 2) $ret = array_multisort($var0,$var1);
	else if ($numArgs == 3) $ret = array_multisort($var0,$var1,$var2);
	else if ($numArgs == 4) $ret = array_multisort($var0,$var1,$var2,$var3);
	else if ($numArgs == 5) $ret = array_multisort($var0,$var1,$var2,$var3,$var4);
	else if ($numArgs == 6) $ret = array_multisort($var0,$var1,$var2,$var3,$var4,$var5);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;
	if ($numArgs >= 4)
		if ($param3 instanceof SuperGlobalOverride) $param3->container = $var3; else $param3 = $var3;
	if ($numArgs >= 5)
		if ($param4 instanceof SuperGlobalOverride) $param4->container = $var4; else $param4 = $var4;
	if ($numArgs >= 6)
		if ($param5 instanceof SuperGlobalOverride) $param5->container = $var5; else $param5 = $var5;

	
	return $ret;
}

function array_pad_override($param0,$param1,$param2)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = array_pad();
	else if ($numArgs == 1) $ret = array_pad($var0);
	else if ($numArgs == 2) $ret = array_pad($var0,$var1);
	else if ($numArgs == 3) $ret = array_pad($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function array_pop_override(&$param0)
{
	$var0;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;

	if ($numArgs == 0) $ret = array_pop();
	else if ($numArgs == 1) $ret = array_pop($var0);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;

	
	return $ret;
}

function array_product_override($param0)
{
	$var0;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;

	if ($numArgs == 0) $ret = array_product();
	else if ($numArgs == 1) $ret = array_product($var0);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;

	
	return $ret;
}

function array_push_override(&$param0,$param1,$param2=null)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = array_push();
	else if ($numArgs == 1) $ret = array_push($var0);
	else if ($numArgs == 2) $ret = array_push($var0,$var1);
	else if ($numArgs == 3) $ret = array_push($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function array_rand_override($param0,$param1=null)
{
	$var0;
	$var1;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;

	if ($numArgs == 0) $ret = array_rand();
	else if ($numArgs == 1) $ret = array_rand($var0);
	else if ($numArgs == 2) $ret = array_rand($var0,$var1);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;

	
	return $ret;
}

function array_reduce_override($param0,$param1,$param2=null)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = array_reduce();
	else if ($numArgs == 1) $ret = array_reduce($var0);
	else if ($numArgs == 2) $ret = array_reduce($var0,$var1);
	else if ($numArgs == 3) $ret = array_reduce($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function array_replace_recursive_override($param0,$param1,$param2=null)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = array_replace_recursive();
	else if ($numArgs == 1) $ret = array_replace_recursive($var0);
	else if ($numArgs == 2) $ret = array_replace_recursive($var0,$var1);
	else if ($numArgs == 3) $ret = array_replace_recursive($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function array_replace_override($param0,$param1,$param2=null)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = array_replace();
	else if ($numArgs == 1) $ret = array_replace($var0);
	else if ($numArgs == 2) $ret = array_replace($var0,$var1);
	else if ($numArgs == 3) $ret = array_replace($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function array_reverse_override($param0,$param1=null)
{
	$var0;
	$var1;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;

	if ($numArgs == 0) $ret = array_reverse();
	else if ($numArgs == 1) $ret = array_reverse($var0);
	else if ($numArgs == 2) $ret = array_reverse($var0,$var1);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;

	
	return $ret;
}

function array_search_override($param0,$param1,$param2=null)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = array_search();
	else if ($numArgs == 1) $ret = array_search($var0);
	else if ($numArgs == 2) $ret = array_search($var0,$var1);
	else if ($numArgs == 3) $ret = array_search($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function array_shift_override(&$param0)
{
	$var0;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;

	if ($numArgs == 0) $ret = array_shift();
	else if ($numArgs == 1) $ret = array_shift($var0);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;

	
	return $ret;
}

function array_slice_override($param0,$param1,$param2=null,$param3=null)
{
	$var0;
	$var1;
	$var2;
	$var3;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;
	if ($numArgs >= 4)
		$var3 = $param3 instanceof SuperGlobalOverride ? $param3->container : $param3;

	if ($numArgs == 0) $ret = array_slice();
	else if ($numArgs == 1) $ret = array_slice($var0);
	else if ($numArgs == 2) $ret = array_slice($var0,$var1);
	else if ($numArgs == 3) $ret = array_slice($var0,$var1,$var2);
	else if ($numArgs == 4) $ret = array_slice($var0,$var1,$var2,$var3);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;
	if ($numArgs >= 4)
		if ($param3 instanceof SuperGlobalOverride) $param3->container = $var3; else $param3 = $var3;

	
	return $ret;
}

function array_splice_override(&$param0,$param1,$param2=null,$param3=null)
{
	$var0;
	$var1;
	$var2;
	$var3;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;
	if ($numArgs >= 4)
		$var3 = $param3 instanceof SuperGlobalOverride ? $param3->container : $param3;

	if ($numArgs == 0) $ret = array_splice();
	else if ($numArgs == 1) $ret = array_splice($var0);
	else if ($numArgs == 2) $ret = array_splice($var0,$var1);
	else if ($numArgs == 3) $ret = array_splice($var0,$var1,$var2);
	else if ($numArgs == 4) $ret = array_splice($var0,$var1,$var2,$var3);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;
	if ($numArgs >= 4)
		if ($param3 instanceof SuperGlobalOverride) $param3->container = $var3; else $param3 = $var3;

	
	return $ret;
}

function array_sum_override($param0)
{
	$var0;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;

	if ($numArgs == 0) $ret = array_sum();
	else if ($numArgs == 1) $ret = array_sum($var0);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;

	
	return $ret;
}

function array_udiff_assoc_override($param0,$param1,$param2)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = array_udiff_assoc();
	else if ($numArgs == 1) $ret = array_udiff_assoc($var0);
	else if ($numArgs == 2) $ret = array_udiff_assoc($var0,$var1);
	else if ($numArgs == 3) $ret = array_udiff_assoc($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function array_udiff_uassoc_override($param0,$param1,$param2,$param3)
{
	$var0;
	$var1;
	$var2;
	$var3;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;
	if ($numArgs >= 4)
		$var3 = $param3 instanceof SuperGlobalOverride ? $param3->container : $param3;

	if ($numArgs == 0) $ret = array_udiff_uassoc();
	else if ($numArgs == 1) $ret = array_udiff_uassoc($var0);
	else if ($numArgs == 2) $ret = array_udiff_uassoc($var0,$var1);
	else if ($numArgs == 3) $ret = array_udiff_uassoc($var0,$var1,$var2);
	else if ($numArgs == 4) $ret = array_udiff_uassoc($var0,$var1,$var2,$var3);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;
	if ($numArgs >= 4)
		if ($param3 instanceof SuperGlobalOverride) $param3->container = $var3; else $param3 = $var3;

	
	return $ret;
}

function array_udiff_override($param0,$param1,$param2)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = array_udiff();
	else if ($numArgs == 1) $ret = array_udiff($var0);
	else if ($numArgs == 2) $ret = array_udiff($var0,$var1);
	else if ($numArgs == 3) $ret = array_udiff($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function array_uintersect_assoc_override($param0,$param1,$param2)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = array_uintersect_assoc();
	else if ($numArgs == 1) $ret = array_uintersect_assoc($var0);
	else if ($numArgs == 2) $ret = array_uintersect_assoc($var0,$var1);
	else if ($numArgs == 3) $ret = array_uintersect_assoc($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function array_uintersect_uassoc_override($param0,$param1,$param2,$param3)
{
	$var0;
	$var1;
	$var2;
	$var3;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;
	if ($numArgs >= 4)
		$var3 = $param3 instanceof SuperGlobalOverride ? $param3->container : $param3;

	if ($numArgs == 0) $ret = array_uintersect_uassoc();
	else if ($numArgs == 1) $ret = array_uintersect_uassoc($var0);
	else if ($numArgs == 2) $ret = array_uintersect_uassoc($var0,$var1);
	else if ($numArgs == 3) $ret = array_uintersect_uassoc($var0,$var1,$var2);
	else if ($numArgs == 4) $ret = array_uintersect_uassoc($var0,$var1,$var2,$var3);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;
	if ($numArgs >= 4)
		if ($param3 instanceof SuperGlobalOverride) $param3->container = $var3; else $param3 = $var3;

	
	return $ret;
}

function array_uintersect_override($param0,$param1,$param2)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = array_uintersect();
	else if ($numArgs == 1) $ret = array_uintersect($var0);
	else if ($numArgs == 2) $ret = array_uintersect($var0,$var1);
	else if ($numArgs == 3) $ret = array_uintersect($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function array_unique_override($param0)
{
	$var0;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;

	if ($numArgs == 0) $ret = array_unique();
	else if ($numArgs == 1) $ret = array_unique($var0);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;

	
	return $ret;
}

function array_unshift_override(&$param0,$param1,$param2=null)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = array_unshift();
	else if ($numArgs == 1) $ret = array_unshift($var0);
	else if ($numArgs == 2) $ret = array_unshift($var0,$var1);
	else if ($numArgs == 3) $ret = array_unshift($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function array_values_override($param0)
{
	$var0;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;

	if ($numArgs == 0) $ret = array_values();
	else if ($numArgs == 1) $ret = array_values($var0);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;

	
	return $ret;
}

function array_walk_recursive_override(&$param0,$param1,$param2=null)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = array_walk_recursive();
	else if ($numArgs == 1) $ret = array_walk_recursive($var0);
	else if ($numArgs == 2) $ret = array_walk_recursive($var0,$var1);
	else if ($numArgs == 3) $ret = array_walk_recursive($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function array_walk_override(&$param0,$param1,$param2=null)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = array_walk();
	else if ($numArgs == 1) $ret = array_walk($var0);
	else if ($numArgs == 2) $ret = array_walk($var0,$var1);
	else if ($numArgs == 3) $ret = array_walk($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function arsort_override(&$param0,$param1=null)
{
	$var0;
	$var1;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;

	if ($numArgs == 0) $ret = arsort();
	else if ($numArgs == 1) $ret = arsort($var0);
	else if ($numArgs == 2) $ret = arsort($var0,$var1);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;

	
	return $ret;
}

function asort_override(&$param0,$param1=null)
{
	$var0;
	$var1;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;

	if ($numArgs == 0) $ret = asort();
	else if ($numArgs == 1) $ret = asort($var0);
	else if ($numArgs == 2) $ret = asort($var0,$var1);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;

	
	return $ret;
}

function compact_override($param0,$param1=null)
{
	$var0;
	$var1;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;

	if ($numArgs == 0) $ret = compact();
	else if ($numArgs == 1) $ret = compact($var0);
	else if ($numArgs == 2) $ret = compact($var0,$var1);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;

	
	return $ret;
}

function count_override($param0,$param1=null)
{
	$var0;
	$var1;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;

	if ($numArgs == 0) $ret = count();
	else if ($numArgs == 1) $ret = count($var0);
	else if ($numArgs == 2) $ret = count($var0,$var1);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;

	
	return $ret;
}

function current_override(&$param0)
{
	$var0;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;

	if ($numArgs == 0) $ret = current();
	else if ($numArgs == 1) $ret = current($var0);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;

	
	return $ret;
}

function each_override(&$param0)
{
	$var0;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;

	if ($numArgs == 0) $ret = each();
	else if ($numArgs == 1) $ret = each($var0);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;

	
	return $ret;
}

function end_override(&$param0)
{
	$var0;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;

	if ($numArgs == 0) $ret = end();
	else if ($numArgs == 1) $ret = end($var0);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;

	
	return $ret;
}

function extract_override(&$param0,$param1=null,$param2=null)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = extract();
	else if ($numArgs == 1) $ret = extract($var0);
	else if ($numArgs == 2) $ret = extract($var0,$var1);
	else if ($numArgs == 3) $ret = extract($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function in_array_override($param0,$param1,$param2=null)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = in_array();
	else if ($numArgs == 1) $ret = in_array($var0);
	else if ($numArgs == 2) $ret = in_array($var0,$var1);
	else if ($numArgs == 3) $ret = in_array($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function key_override(&$param0)
{
	$var0;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;

	if ($numArgs == 0) $ret = key();
	else if ($numArgs == 1) $ret = key($var0);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;

	
	return $ret;
}

function krsort_override(&$param0,$param1=null)
{
	$var0;
	$var1;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;

	if ($numArgs == 0) $ret = krsort();
	else if ($numArgs == 1) $ret = krsort($var0);
	else if ($numArgs == 2) $ret = krsort($var0,$var1);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;

	
	return $ret;
}

function ksort_override(&$param0,$param1=null)
{
	$var0;
	$var1;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;

	if ($numArgs == 0) $ret = ksort();
	else if ($numArgs == 1) $ret = ksort($var0);
	else if ($numArgs == 2) $ret = ksort($var0,$var1);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;

	
	return $ret;
}

function natcasesort_override(&$param0)
{
	$var0;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;

	if ($numArgs == 0) $ret = natcasesort();
	else if ($numArgs == 1) $ret = natcasesort($var0);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;

	
	return $ret;
}

function natsort_override(&$param0)
{
	$var0;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;

	if ($numArgs == 0) $ret = natsort();
	else if ($numArgs == 1) $ret = natsort($var0);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;

	
	return $ret;
}

function next_override(&$param0)
{
	$var0;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;

	if ($numArgs == 0) $ret = next();
	else if ($numArgs == 1) $ret = next($var0);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;

	
	return $ret;
}

function pos_override(&$param0)
{
	$var0;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;

	if ($numArgs == 0) $ret = pos();
	else if ($numArgs == 1) $ret = pos($var0);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;

	
	return $ret;
}

function prev_override(&$param0)
{
	$var0;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;

	if ($numArgs == 0) $ret = prev();
	else if ($numArgs == 1) $ret = prev($var0);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;

	
	return $ret;
}

function range_override($param0,$param1,$param2=null)
{
	$var0;
	$var1;
	$var2;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;
	if ($numArgs >= 3)
		$var2 = $param2 instanceof SuperGlobalOverride ? $param2->container : $param2;

	if ($numArgs == 0) $ret = range();
	else if ($numArgs == 1) $ret = range($var0);
	else if ($numArgs == 2) $ret = range($var0,$var1);
	else if ($numArgs == 3) $ret = range($var0,$var1,$var2);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;
	if ($numArgs >= 3)
		if ($param2 instanceof SuperGlobalOverride) $param2->container = $var2; else $param2 = $var2;

	
	return $ret;
}

function reset_override(&$param0)
{
	$var0;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;

	if ($numArgs == 0) $ret = reset();
	else if ($numArgs == 1) $ret = reset($var0);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;

	
	return $ret;
}

function rsort_override(&$param0,$param1=null)
{
	$var0;
	$var1;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;

	if ($numArgs == 0) $ret = rsort();
	else if ($numArgs == 1) $ret = rsort($var0);
	else if ($numArgs == 2) $ret = rsort($var0,$var1);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;

	
	return $ret;
}

function shuffle_override(&$param0)
{
	$var0;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;

	if ($numArgs == 0) $ret = shuffle();
	else if ($numArgs == 1) $ret = shuffle($var0);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;

	
	return $ret;
}

function sizeof_override($param0,$param1=null)
{
	$var0;
	$var1;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;

	if ($numArgs == 0) $ret = sizeof();
	else if ($numArgs == 1) $ret = sizeof($var0);
	else if ($numArgs == 2) $ret = sizeof($var0,$var1);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;

	
	return $ret;
}

function sort_override(&$param0,$param1=null)
{
	$var0;
	$var1;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;

	if ($numArgs == 0) $ret = sort();
	else if ($numArgs == 1) $ret = sort($var0);
	else if ($numArgs == 2) $ret = sort($var0,$var1);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;

	
	return $ret;
}

function uasort_override(&$param0,$param1)
{
	$var0;
	$var1;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;

	if ($numArgs == 0) $ret = uasort();
	else if ($numArgs == 1) $ret = uasort($var0);
	else if ($numArgs == 2) $ret = uasort($var0,$var1);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;

	
	return $ret;
}

function uksort_override(&$param0,$param1)
{
	$var0;
	$var1;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;

	if ($numArgs == 0) $ret = uksort();
	else if ($numArgs == 1) $ret = uksort($var0);
	else if ($numArgs == 2) $ret = uksort($var0,$var1);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;

	
	return $ret;
}

function usort_override(&$param0,$param1)
{
	$var0;
	$var1;

	$ret;
	
	$numArgs = func_num_args();

	if ($numArgs >= 1)
		$var0 = $param0 instanceof SuperGlobalOverride ? $param0->container : $param0;
	if ($numArgs >= 2)
		$var1 = $param1 instanceof SuperGlobalOverride ? $param1->container : $param1;

	if ($numArgs == 0) $ret = usort();
	else if ($numArgs == 1) $ret = usort($var0);
	else if ($numArgs == 2) $ret = usort($var0,$var1);


	if ($numArgs >= 1)
		if ($param0 instanceof SuperGlobalOverride) $param0->container = $var0; else $param0 = $var0;
	if ($numArgs >= 2)
		if ($param1 instanceof SuperGlobalOverride) $param1->container = $var1; else $param1 = $var1;

	
	return $ret;
}

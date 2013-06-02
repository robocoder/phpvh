
$annotationArray = NULL;

$annotationHandle = NULL;

function Annotation($id)
{
	global $annotationArray;
	global $annotationHandle;
	
	if ($annotationHandle == NULL) {
		$annotationHandle = fopen($_SERVER['DOCUMENT_ROOT'] . '/annotation.txt', 'a') or die('error opening annotation file');
		/*$annotationArray = explode("\n", $contents);*/
	}
	
	/*
	if (in_array($id, $annotationArray))
		return;	
	
		
	array_push($annotationArray, $id);
	*/
		
	fwrite($annotationHandle, $id . "\n");
		
	//fclose($f);
}

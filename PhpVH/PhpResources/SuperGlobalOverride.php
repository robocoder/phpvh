
class SuperGlobalOverride implements arrayaccess, Iterator {
	public $container = array();
	private $superGlobalName;
	private $hookFunction;
	private $position;
    public function __construct($ActualSuperGlobal, $SuperGlobalName) {
		$this->container = $ActualSuperGlobal;
		$this->superGlobalName = $SuperGlobalName;
		$this->position = 0;
    }
	
	private function LogValue($offset, $value)
	{
		
		/*if ($this->superGlobalName == '$_GET' ||
			$this->superGlobalName == '$_POST' || 
			$this->superGlobalName == '$_REQUEST' ||
			$this->superGlobalName == '$_COOKIE')
			$value = '';*/
		
	
	
		//echo "<br />Get " . $this->superGlobalName . "<br />";
		
		$f = fopen($_SERVER['DOCUMENT_ROOT'] . '/trace.txt', 'a') or die('error opening trace file');
		
		fwrite($f, "--------------------------------Start\n" .
			"Function Called: " . $this->superGlobalName . "\n" .
			'$Param0: '.$offset."\n" .
			"Value: " . $value . "\n" .
			"--------------------------------End\n");
			
		fclose($f);
	}
	
    public function offsetSet($offset, $value) {
		$this->LogValue($offset, $value);
	
        if (is_null($offset)) {
            $this->container[] = $value;
        } else {
            $this->container[$offset] = $value;
        }
    }
    public function offsetExists($offset) {
		$this->LogValue($offset, $this->offsetGet($offset));
    
        return isset($this->container[$offset]);
    }
    public function offsetUnset($offset) {
		$this->LogValue($offset, $this->offsetGet($offset));
    
        unset($this->container[$offset]);
    }
    public function offsetGet($offset) {
		$value = isset($this->container[$offset]) ? $this->container[$offset] : null;
	
		$this->LogValue($offset, $value);
		
        return $value;
    }
	
	private function getKeys() {
		return array_keys($this->container);
	}
	
	function rewind() {
		//echo 'rewind called<br />';
        $this->position = 0;
    }

    function current() {
		//echo 'current called<br />';
		$keys = $this->getKeys();
        return $this->container[$keys[$this->position]];
    }

    function key() {
		//echo 'key called<br />';
        return $this->position;
    }

    function next() {
	//echo 'next called<br />';
        ++$this->position;
    }

    function valid() {
		//echo 'valid called<br />';
		$keys = $this->getKeys();
        return isset($keys[$this->position]) && isset($this->container[$keys[$this->position]]);
    }
}

$_GET = new SuperGlobalOverride($_GET, '$_GET');
$_POST = new SuperGlobalOverride($_POST, '$_POST');
$_REQUEST = new SuperGlobalOverride($_REQUEST, '$_REQUEST');
$_COOKIE = new SuperGlobalOverride($_COOKIE, '$_COOKIE');


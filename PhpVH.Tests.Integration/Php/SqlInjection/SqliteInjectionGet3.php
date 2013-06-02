<?php
sqlite_array_query("handle", "SELECT * FROM table WHERE field='$_GET[query]'");
?>
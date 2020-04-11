# ControlCollection.Item Property 
 

Gets the <a href="eae0acea-bdd1-dc08-7fda-dcd25c5f2082">ConsoleControl</a> at the given *index*.

**Namespace:**&nbsp;<a href="8161a036-2926-0ace-99d3-20346d250e3b">ConControls.Controls</a><br />**Assembly:**&nbsp;ConControls (in ConControls.dll) Version: 0.1.0-alpha

## Syntax

**C#**<br />
``` C#
public ConsoleControl this[
	int index
] { get; }
```

**VB**<br />
``` VB
Public ReadOnly Default Property Item ( 
	index As Integer
) As ConsoleControl
	Get
```

**C++**<br />
``` C++
public:
property ConsoleControl^ default[int index] {
	ConsoleControl^ get (int index);
}
```

**F#**<br />
``` F#
member Item : ConsoleControl with get

```


#### Parameters
&nbsp;<dl><dt>index</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.int32" target="_blank">System.Int32</a><br />The index of the <a href="eae0acea-bdd1-dc08-7fda-dcd25c5f2082">ConsoleControl</a> in this collection.</dd></dl>

#### Return Value
Type: <a href="eae0acea-bdd1-dc08-7fda-dcd25c5f2082">ConsoleControl</a><br />The <a href="eae0acea-bdd1-dc08-7fda-dcd25c5f2082">ConsoleControl</a> at the given *index*.

## Exceptions
&nbsp;<table><tr><th>Exception</th><th>Condition</th></tr><tr><td><a href="https://docs.microsoft.com/dotnet/api/system.indexoutofrangeexception" target="_blank">IndexOutOfRangeException</a></td><td>The *index* was outside this collection.</td></tr></table>

## See Also


#### Reference
<a href="72e613b7-790f-5a58-b25d-f7e6b12dcdce">ControlCollection Class</a><br /><a href="8161a036-2926-0ace-99d3-20346d250e3b">ConControls.Controls Namespace</a><br />
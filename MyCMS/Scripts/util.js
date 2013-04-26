//将文本转换为节点
function checkElem(elem)
{
    return elem && elem.constructor == String ? document.createTextNode(elem) : elem;
}

function create(elem)
{
    return document.createElementNS ?
        document.createElementNS('http://www.w3.org/1999/xhtml', elem) :
        document.createElement(elem);
}

function append(parent, elem)
{
    parent.appendChild(checkElem(elem));
}

function last(elem)
{

    elem = elem.lastChild;

    return elem && elem.nodeType != 1 ? prev(elem) : elem;
}

function id(name)
{
    return typeof name == "string" ? document.getElementById(name) : name;
}



function setStyle(elements, prop, val) {

	for (var i = 0, len = elements.length; i < len; i++) {

		if(typeof elements[i] == "string")
			document.getElementById(elements[i]).style[prop] = val;
		else
			elements[i].style[prop] = val;

    	}
}

function setCSS(el, styles) {
    for (var prop in styles) {
	    if (!styles.hasOwnProperty(prop)) continue;
	    
        setStyle(el, prop, styles[prop]);
    }
}

//注册命名空间
function nameSpace(a, b)
{
    var c = a.split(/\./);
    var d = window;
    for (var e = 0; e < c.length - 1; e++)
    {


        if (!d[c[e]])
        {

            d[c[e]] = {};
        }

        d = d[c[e]];
    }

    d[c[c.length - 1]] = b;
}
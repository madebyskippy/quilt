var lines;

function readtxt(){
  lines = loadStrings('pattern'+str(file)+'.txt');
  print("there are " + lines.length + " lines");
  parsetxt();
}

function parsetxt(){
  var polyfill = new Array(lines.length);//new int[lines.length][];
  var index = 0;
  for (var i = 0 ; i < lines.length; i++) {
    var l = split(lines[i],",");
    polyfill[i] = new Array(l.length);
    for (var j=0; j<polyfill[i].length; j++){
      polyfill[i][j] = parseInt(l[j]);
    }
  }
  return polyfill;
}

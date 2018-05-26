var lines;

function readtxt(){
  print("reading");
  print(file);
  jQuery.get('pattern'+str(file)+'.txt', function(data) {
    lines = data.split("\n");
    print(lines);
    poly = parsetxt();
  });
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
  touched = new Array(polyfill.length);
  print("ye");
  return polyfill;
}

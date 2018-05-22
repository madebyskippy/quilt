function jiggle(){
  var threshold = 1.5;
  var increment = 0.5;
  for (var i=0; i<r; i++){
    for (var j=0; j<c; j++){
      var index = i*r+j;
      var x,y;
      if (points [index].x - stable [index].x > threshold) {
        x = round(random (-1, 0)) * increment;
      } else if (stable [index].x - points [index].x > threshold) {
        x = round(random (0, 1)) * increment;
      } else {
        x = round(random (-1, 1)) * increment;
      }
      if (points [index].y - stable [index].y > threshold) {
        y = round(random (-1, 0)) * increment;
      } else if (stable [index].y - points [index].y > threshold) {
        y = round(random (0, 1)) * increment;
      } else {
        y = round(random (-1, 1)) * increment;
      }
      points[index].x+=x;
      points[index].y+=y;
    }
  }
}

function warp(){
  var increment = 0.5;
  for (var i=0; i<r; i++){
    for (var j=0; j<c; j++){
      var x,y;
      x = random(-1,1) * increment;
      y = random(-1,1) * increment;
      points[i*r+j].x+=x;
      points[i*r+j].y+=y;
    }
  }
}

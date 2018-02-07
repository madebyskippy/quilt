void jiggle(){
  float threshold = 1.5f;
  float increment = 0.5f;
  for (int i=0; i<r; i++){
    for (int j=0; j<c; j++){
      int index = i*r+j;
      float x,y;
      if (points [index].x - stable [index].x > threshold) {
          x = random (-1f, 0) * increment;
        } else if (stable [index].x - points [index].x > threshold) {
          x = random (0, 1f) * increment;
        } else {
          x = random (-1f, 1f) * increment;
        }
        if (points [index].y - stable [index].y > threshold) {
          y = random (-1f, 0) * increment;
        } else if (stable [index].y - points [index].y > threshold) {
          y = random (0, 1f) * increment;
        } else {
          y = random (-1f, 1f) * increment;
        }
        points[index].add(new PVector(x,y));
    }
  }
}

void warp(){
  float increment = 0.5f;
  for (int i=0; i<r; i++){
    for (int j=0; j<c; j++){
      float x,y;
      x = random(-1f,1f) * increment;
      y = random(-1f,1f) * increment;
      points[i*r+j].add(new PVector(x,y));
    }
  }
}
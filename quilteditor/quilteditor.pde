int r=20; int c=20;
PVector[] points = new PVector[r*c];
PVector[] stable = new PVector[r*c];
ArrayList<int[]> poly = new ArrayList<int[]>();

IntList currentpoly = new IntList();

PVector selected;

void setup(){
  selected = new PVector(0,0);
  background(255);
  size(675,675);
  for (int i=0; i<r; i++){
    for (int j=0; j<c; j++){
      int index = i*r+j;
      points[index] = new PVector(i*25+100,j*25+100);
      stable[index] = new PVector(i*25+100,j*25+100);
    }
  }
}

void draw() {
  background(255);
  noFill();
  drawquads();
  drawpoints();
}

void drawpoints(){
  float radius = 1;
  noStroke();
  for (int i=0; i<r; i++){
    for (int j=0; j<c; j++){
      int index = i*r+j;
      if (i == selected.x && j == selected.y){
        fill(150,0,0);
        radius = 5;
      }else{
        fill(150);
        radius = 3;
      }
      ellipse(points[index].x,points[index].y,radius,radius);
    }
  }
}

void drawquads(){
  stroke(0);
  fill(225);
  for (int i=0; i<poly.size(); i++){
    beginShape();
    for (int j=0; j<poly.get(i).length;j++){
      vertex(points[poly.get(i)[j]].x,points[poly.get(i)[j]].y);
    }
    endShape(CLOSE);
  }
}

void savetext(){
  String[] polies = new String[poly.size()];
  for (int i=0; i<poly.size(); i++){
    polies[i] = join(nf(poly.get(i),0),",");
  }
  saveStrings("pattern.txt" , polies);
  // Writes the strings to a file, each on a separate line
  //saveStrings("nouns.txt", list);
}

void keyPressed(){
  if (key==CODED){
    if (keyCode == UP){
      selected.y = (selected.y-1)%c;
    }else if (keyCode == DOWN){
      selected.y = (selected.y+1)%c;
    }else if (keyCode == LEFT){
      selected.x = (selected.x-1)%r;
    }else if (keyCode == RIGHT){
      selected.x = (selected.x+1)%r;
    }
    
    if (selected.x < 0){
      selected.x=r-1;
    }if (selected.y < 0){
      selected.y=c-1;
    }
  }else{
    if (key == 'x'){
      if (currentpoly.size()>2){
        poly.add(currentpoly.array());
      }
      currentpoly.clear();
      int index = (int)selected.x*r+(int)selected.y;
      currentpoly.append(index);
    }else if  (key == 'c'){
      if (currentpoly.size()>2){
        poly.add(currentpoly.array());
      }
      currentpoly.clear();
    }else if (key == 'z'){
      int index = (int)selected.x*r+(int)selected.y;
      currentpoly.append(index);
    }else if (key == 'q'){
      savetext();
    }
  }
}
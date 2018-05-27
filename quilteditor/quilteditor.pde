int r=20; int c=20;
PVector[] points = new PVector[r*c];
PVector[] stable = new PVector[r*c];
ArrayList<int[]> poly = new ArrayList<int[]>();

IntList currentpoly = new IntList();

PVector selected;

int saveindex;

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
  
  saveindex = 0;
  
  PFont font;
  font = loadFont("Monospaced-48.vlw");
  textFont(font, 13);
  textAlign(CENTER,TOP);
}

void draw() {
  background(255);
  noFill();
  drawquads();
  drawpoints();
  //draw current poly
  
  stroke(0);
  beginShape();
  for (int i=0; i<currentpoly.size(); i++){
    vertex(points[currentpoly.get(i)].x,points[currentpoly.get(i)].y);
    ellipse(points[currentpoly.get(i)].x,points[currentpoly.get(i)].y,5,5);
  }
  endShape(CLOSE);
  
  text("Z to start a new poly and then add points\nC to end it // X to end it and start anew\narrows to move points\nq to save the level",width/2,20);
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
  saveStrings("pattern"+str(saveindex)+".txt" , polies);
  save("pattern"+str(saveindex)+".png");
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
      //finish poly and start new one
      if (currentpoly.size()>2){
        poly.add(currentpoly.array());
      }
      currentpoly.clear();
      int index = (int)selected.x*r+(int)selected.y;
      currentpoly.append(index);
    }else if  (key == 'c'){
      //finish poly
      if (currentpoly.size()>2){
        poly.add(currentpoly.array());
      }
      currentpoly.clear();
    }else if (key == 'z'){
      //start poly
      int index = (int)selected.x*r+(int)selected.y;
      currentpoly.append(index);
    }else if (key == 'q'){
      //finish level
      savetext();
    }
  }
}

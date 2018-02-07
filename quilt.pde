int r=20; int c=20;
int size = 25;
int offset;
PVector[] points = new PVector[r*c];
PVector[] stable = new PVector[r*c];
int[][] poly;

boolean[] touched;
int touchcount;
boolean pointtouch;
int pointtouched;
float pointthresh = 7.5f;

PVector mouse;
boolean mousedown;
boolean playing;

int file = 0;
int numfiles = 3;

int time;

void setup(){
  PFont font;
  font = loadFont("Monospaced-48.vlw");
  textFont(font, 16);
  textAlign(CENTER,CENTER);
  
  mousedown=false;
  playing = false;
  time = 0;
  pointtouched=-1;
  
  offset = 112;
  
  background(255);
  size(675,675);
  
  for (int i=0; i<r; i++){
    for (int j=0; j<c; j++){
      int index = i*r+j;
      points[index] = new PVector(i*size+offset,j*size+offset);
      stable[index] = new PVector(i*size+offset,j*size+offset);
    }
  }
  readtxt();
  poly = parsetxt();
  touched = new boolean[poly.length];
  for (int i=0; i<touched.length; i++){
    touched[i] = false;
  }
  touchcount=0;
  pointtouch=false;
}

void draw() {
  noFill();
  
  if (playing){
    background(255);
    warp();
    
    //check what you touched
    for (int i=0; i<poly.length; i++){
      if (collision(poly[i],new PVector(mouseX,mouseY))){
        if (!touched[i]){
          touchcount += 1;
        }
        touched[i]=true;
      }
    }
    //check if you touched a point
    if (pointtouch){
      playing = false;
    }
    //check if you touched enough
    if (touchcount >= poly.length){
      playing = false;
    }
    
  }else{
    background(235);
    noStroke();
    fill(0);
    text("touch all spaces\navoid all intersections\nclick to start",width/2,size*2.5);
    jiggle();
  }
  drawquads();
  if (pointtouched != -1){
    fill(255,0,0,100);
    noStroke();
    ellipse(points[pointtouched].x,points[pointtouched].y,
            pointthresh*2,pointthresh*2);
  }
  noStroke();
  fill(0);
  text(str(time),width/2,height-size*2.5);
}

void drawpoints(){
  float radius = pointthresh;
  noStroke();
  fill(255,0,0,100);
  for (int i=0; i<r; i++){
    for (int j=0; j<c; j++){
      int index = i*r+j;
      ellipse(points[index].x,points[index].y,radius,radius);
    }
  }
}

void drawquads(){
  stroke(0);
  fill(225);
  for (int i=0; i<poly.length; i++){
    if (touched[i]){
      fill(150);
    }else{
      fill(250);
    }
    beginShape();
    for (int j=0; j<poly[i].length;j++){
      vertex(points[poly[i][j]].x,points[poly[i][j]].y);
    }
    endShape(CLOSE);
  }
}

//returns true if PVector point m is inside of int[] poly p
boolean collision(int[] p, PVector m){
  boolean c=false;
  for (int i=0; i<p.length; i++){
    //for poly checking
    PVector vc = points[p[i]];
    PVector vn = points[p[0]];
    if (i+1 < p.length){
      vn = points[p[i+1]];
    }
    if ( ((vc.y > m.y) != (vn.y > m.y)) &&
          (m.x < (vn.x-vc.x)*(m.y-vc.y) / (vn.y-vc.y) + vc.x) ){
      c = !c;
    }
    
    //for point checking
    float distance = sqrt(pow(m.x-vc.x,2)+pow(m.y-vc.y,2));
    if (distance < pointthresh){
      pointtouch = true;
      pointtouched = p[i];
      break;
    }
  }
  return c;
}

void mousePressed(){
  //mouse just got pressed
  if (mouseButton == LEFT){
    if (!mousedown){
      playing = true;
    }
    mousedown = true;
  }
}
void mouseReleased(){
  //mouse just got released
  if (mouseButton == LEFT){
    mousedown = false;
  }
}

void keyPressed(){
  if (key == 'r'){
    setup();
  }
  for (int i=0; i<numfiles; i++){
    if (key == str(i).charAt(0)){
      file = i;
      setup();
    }
  }
}
//local high score (like alphabet game) -- need to test OK IT DOESN'T WORK

var r=20; var c=20;
var gridsize = 25;
var offsetx,offsety;
var points;//PVector[] points;
var stable;//PVector[] stable;
var poly;//int[][] poly;

var touched;//boolean[] touched;
var touchcount;
var pointtouch;
var pointtouched;
var pointthresh;
var done;

var mouse;
var mousedown;
var playing;

var currentlevelhover = -1;

var file = 0;
var numfiles = 9;

var starttime;
var time;
var scores;

function preload(){
  scores = new Array(0,0,0,0,0,0,0,0,0);
  for (var i=0; i<numfiles; i++){
    if (localStorage.getItem(str(i)) != null){
      scores[i] = localStorage.getItem(str(i));
    }
  }
  print(scores);
  lines = new Array();
  touched = new Array();
  poly = new Array();
  readtxt();
}

function setup(){
  var font;
  font = loadFont("assets/SpaceMono-Regular.ttf");
  textFont(font, 16);
  
  offsetx = windowWidth/2-c/2*gridsize+gridsize;
  offsety = windowHeight/2-r/2*gridsize+gridsize;
  
  background(255);
  createCanvas(windowWidth, windowHeight);
  
  points = new Array(r*c);//tuple[r*c];
  stable = new Array(r*c);//tuple[r*c];
  
  for (var i=0; i<r; i++){
    for (var j=0; j<c; j++){
      var index = i*r+j;
      points[index] = new tuple;
      points[index].tuple(i*gridsize+offsetx,j*gridsize+offsety);
      stable[index] = new tuple;
      stable[index].tuple(i*gridsize+offsetx,j*gridsize+offsety);
    }
  }
  textAlign(CENTER,CENTER);
  
  //poly = parsetxt();
  
  reset();
}

function reset(){
  mousedown=false;
  playing = false;
  time = 0;
  pointtouched=-1;
  currentlevelhover = -1;
  touchcount=0;
  pointtouch=false;
  pointthresh = 5;
  for (var i=0; i<touched.length; i++){
    touched[i] = false;
  }
  done = false;
  readtxt();
}

function draw() {
  noFill();
  
  if (playing){
    background(255);
    warp();
    
    //check what you touched
    for (var i=0; i<poly.length; i++){
      var t = new tuple;
      t.tuple(mouseX,mouseY);
      if (collision(poly[i],t)){
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
      scores[file] = round(time/1000);
      localStorage.setItem(str(file),str(scores[file]));
      playing = false;
      done = true;
    }
    
    time = millis() - starttime;
    
  }else{
    background(235);
    noStroke();
    fill(0);
    var textY = windowHeight/2-c/2*gridsize-gridsize*1/2;
    textAlign(CENTER,BOTTOM);
    if (done){
      text("good work\n\nyou got it in "+str(round(time/1000))+" seconds",width/2,textY);
    }else{
      text("-click to start-\n\n\ntouch all spaces\navoid all intersections",width/2,textY);
    }
    jiggle();
  }
  drawquads();
  if (pointtouched != -1){
    stroke(0);
    noFill();
    ellipse(points[pointtouched].x,points[pointtouched].y,
            pointthresh*3,pointthresh*3);
    ellipse(points[pointtouched].x,points[pointtouched].y,
            pointthresh*2,pointthresh*2);
    ellipse(points[pointtouched].x,points[pointtouched].y,
            pointthresh,pointthresh);
  }
  drawlevelselect();
  noStroke();
  fill(0);
  textAlign(CENTER,TOP);
  text(str(round(time/1000)),width/2,windowHeight/2+r/2*gridsize+gridsize/2);
}

function drawpoints(){
  var radius = pointthresh;
  noStroke();
  fill(255,0,0,100);
  for (var i=0; i<r; i++){
    for (var j=0; j<c; j++){
      var index = i*r+j;
      ellipse(points[index].x,points[index].y,radius,radius);
      ellipse(points[index].x,points[index].y,pointthresh,pointthresh);
    }
  }
}

function drawquads(){
  stroke(0);
  fill(225);
  for (var i=0; i<poly.length; i++){
    if (done){
      stroke(255);
      fill(0);
    }else if(touched[i]){
      fill(150);
    }else{
      fill(250);
    }
    beginShape();
    for (var j=0; j<poly[i].length;j++){
      vertex(points[poly[i][j]].x,points[poly[i][j]].y);
    }
    endShape(CLOSE);
  }
}

function drawlevelselect(){
  textAlign(CENTER,CENTER);
  noFill();
  var h = 30;
  var r = 40;
  var total = width*0.8;
  var part = total / numfiles;
  var x = 0.1*width;
  var yoffset = 5;
  currentlevelhover = -1;
  for (var i=0; i<numfiles; i++){
    noFill();
    stroke(0);
    
    if (mouseY<height && mouseY>(height-r-yoffset)){
      if (mouseX > (x+part/2-r/2) && mouseX < (x+part/2+r/2-1)){
        currentlevelhover=i;
        fill(200);
      }
    }
    if (i == file){
      fill(170);
    }
    
    arc(x+part/2,height-r/2-yoffset,r,r,PI,0);
    //rect(x+part/2-r/2,height-r,r,r/2);
    noStroke();
    rect(x+part/2-r/2,height-r/2-yoffset,r,r);
    stroke(0);
    line(x+part/2-r/2,height-r/2-yoffset,x+part/2-r/2,height);
    line(x+part/2+r/2-1,height-r/2-yoffset,x+part/2+r/2-1,height);
    fill(0);
    noStroke();
    text(str(i+1),x+part/2,height-35);
    textSize(13);
    text(str(scores[i])+"s",x+part/2,height-15);
    textSize(16);
    x += part;
  }
}

//returns true if PVector point m is inside of int[] poly p
//http://jeffreythompson.org/collision-detection/poly-point.php
function collision(p, m){
  var c=false;
  for (var i=0; i<p.length; i++){
    //for poly checking
    var vc = points[p[i]];
    var vn = points[p[0]];
    if (i+1 < p.length){
      vn = points[p[i+1]];
    }
    if ( ((vc.y > m.y) != (vn.y > m.y)) &&
          (m.x < (vn.x-vc.x)*(m.y-vc.y) / (vn.y-vc.y) + vc.x) ){
      c = !c;
    }
    
    //for point checking
    var distance = sqrt(pow(m.x-vc.x,2)+pow(m.y-vc.y,2));
    if (distance < pointthresh){
      pointtouch = true;
      pointtouched = p[i];
      break;
    }
  }
  return c;
}

function mousePressed(){
  //mouse just got pressed
  if (mouseButton == LEFT){
    print("clicked");
    if (!mousedown){
      if (currentlevelhover < 0){
        if (!playing){
          if (pointtouch){
            reset();
            playing = true;
            starttime = millis();
          }else if (done){
            reset();
          }else{
            playing = true;
            starttime = millis();
          }
        }
      }else{
        file = currentlevelhover;
        reset();
      }
    }
    mousedown = true;
  }
}
function mouseReleased(){
  //mouse just got released
    print("release");
  if (mouseButton == LEFT){
    mousedown = false;
  }
}

function keyPressed(){
  print("pressed key "+key);
  if (key == 'r' || key == 'R'){
    reset();
  }
  for (var i=1; i<=numfiles; i++){
    if (key == str(i).charAt(0)){
      file = i-1;
      reset();
    }
  }
}

function tuple(){
  this.x=0;
  this.y=0;
  this.tuple = function(j, k){
    this.x=j;
    this.y=k;
  }
}

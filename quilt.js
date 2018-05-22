var r=20; var c=20;
var gridsize = 25;
var offset;
var points;//PVector[] points;
var stable;//PVector[] stable;
var poly;//int[][] poly;

var touched;//boolean[] touched;
var touchcount;
var pointtouch;
var pointtouched;
var pointthresh;

var mouse;
var mousedown;
var playing;

var currentlevelhover = -1;

var file = 0;
var numfiles = 3;

var starttime;
var time;

function preload(){
  //readtxt();
}

function setup(){
  var font;
  font = loadFont("assets/SpaceMono-Regular.ttf");
  textFont(font, 16);
  
  mousedown=false;
  playing = false;
  time = 0;
  pointtouched=-1;
  currentlevelhover = -1;
  touchcount=0;
  pointtouch=false;
  pointthresh = 7.5;
  
  offset = 112;
  
  background(255);
  //size(675,675);
  createCanvas(675,675);
  
  
  points = new Array(r*c);//tuple[r*c];
  stable = new Array(r*c);//tuple[r*c];
  
  for (var i=0; i<r; i++){
    for (var j=0; j<c; j++){
      var index = i*r+j;
      points[index] = new tuple;
      points[index].tuple(i*gridsize+offset,j*gridsize+offset);
      stable[index] = new tuple;
      stable[index].tuple(i*gridsize+offset,j*gridsize+offset);
    }
  }
  textAlign(CENTER,CENTER);
  
  poly = parsetxt();
  touched = new Array(poly.length);//boolean[poly.length];
  for (var i=0; i<touched.length; i++){
    touched[i] = false;
  }
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
      playing = false;
    }
    
    time = millis() - starttime;
    
  }else{
    background(235);
    noStroke();
    fill(0);
    text("touch all spaces\navoid all intersections\nclick to start",width/2,gridsize*2.5);
    jiggle();
  }
  drawquads();
  if (pointtouched != -1){
    fill(255,0,0,100);
    noStroke();
    ellipse(points[pointtouched].x,points[pointtouched].y,
            pointthresh*2,pointthresh*2);
  }
  drawlevelselect();
  noStroke();
  fill(0);
  text(str(round(time/1000)),width/2,height-gridsize*3);
}

function drawpoints(){
  var radius = pointthresh;
  noStroke();
  fill(255,0,0,100);
  for (var i=0; i<r; i++){
    for (var j=0; j<c; j++){
      var index = i*r+j;
      ellipse(points[index].x,points[index].y,radius,radius);
    }
  }
}

function drawquads(){
  stroke(0);
  fill(225);
  for (var i=0; i<poly.length; i++){
    if (touched[i]){
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
  noFill();
  var h = 30;
  var r = 40;
  var total = width*0.8;
  var part = total / numfiles;
  var x = 0.1*width;
  currentlevelhover = -1;
  for (var i=0; i<numfiles; i++){
    noFill();
    stroke(0);
    if (mouseY<height){
      var dist = sqrt(pow(mouseX-(x+part/2),2)+pow(mouseY-(height-r/4),2));
      if (dist < r/2){
        currentlevelhover=i;
        fill(200);
      }
    }
    arc(x+part/2,height-r/4,r,r,0,4*PI);
    fill(0);
    noStroke();
    text(str(i+1),x+part/2,height-15);
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
            setup();
          }else{
            playing = true;
            starttime = millis();
          }
        }
      }else{
        file = currentlevelhover;
        setup();
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
  if (key == 'r'){
    setup();
  }
  for (var i=0; i<numfiles; i++){
    if (key == str(i).charAt(0)){
      file = i;
      setup();
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

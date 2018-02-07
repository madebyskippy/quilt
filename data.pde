String[] lines;

void readtxt(){
  lines = loadStrings("pattern"+str(file)+".txt");
  println("there are " + lines.length + " lines");
  parsetxt();
}

int[][] parsetxt(){
  int[][] poly = new int[lines.length][];
  for (int i = 0 ; i < lines.length; i++) {
    poly[i] = int(split(lines[i],","));
  }
  return poly;
}
﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Msagl = Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;

namespace Corruption
{
    class Graph
    {
        public Dictionary<string, City> cities;
        public string startCity;
        public List<Edge> infectedEdge;

        public Graph()
        {
            this.cities = new Dictionary<string, City>();
            this.infectedEdge = new List<Edge>();
        }

        public void BFS(int time)
        {
            this.cities[this.startCity].timeInfected = 0;
            this.cities[this.startCity].isInfected = true;

            Queue<Edge> VirusInfector = new Queue<Edge>();

            foreach (City c in this.cities[startCity].neighbors.Keys)
            {
                VirusInfector.Enqueue(new Edge(this.cities[startCity], this.cities[c.name]));
            }

            City currentCity;
            City targetCity;
            Edge cityTuple;
            int t;
            int timeInfectedTemp;

            while (VirusInfector.Count > 0)
            {
                cityTuple = VirusInfector.Dequeue();
                currentCity = cityTuple.fromCity;
                targetCity = cityTuple.toCity;

                if (currentCity.CanInfect(targetCity, time))
                {
                    t = currentCity.GetDayTargetInfected(targetCity);
                    timeInfectedTemp = currentCity.timeInfected + t;
                    if (timeInfectedTemp < targetCity.timeInfected)
                    {
                        foreach (City c in this.cities[targetCity.name].neighbors.Keys)
                        {
                            VirusInfector.Enqueue(new Edge(this.cities[targetCity.name], this.cities[c.name]));
                        }
                        targetCity.timeInfected = timeInfectedTemp;
                        targetCity.isInfected = true;
                    }
                    infectedEdge.Add(cityTuple); //tambahin edge yang merupakan edge virus
                }
            }
        }

       
        public GViewer Visualize(int t)
        {
            //create a form 
            Form form = new Form();

            //create a viewer object 
            GViewer viewer = new GViewer();

            //create a graph object 
            Msagl.Graph graph = new Msagl.Graph("Corruption");

            //create the graph content
            foreach (City city in this.cities.Values)
            {
                graph.AddNode(city.name);
                if (city.isInfected)
                {
                    int red = 255, other = 255;
                    if (city.isInfected)
                    {
                        red = 250;
                        other = 150 - (int)(city.InfectedPercentage(t) * 150);
                    }
                    graph.FindNode(city.name).Attr.FillColor = new Msagl.Color(255, (byte)red, (byte)other, (byte)other);
                }

                foreach (City neigh in city.neighbors.Keys)
                {
                    bool tmp = false;
                    foreach (Edge e in this.infectedEdge)
                    {
                        if ((e.fromCity == city && e.toCity == neigh))
                        {
                            tmp = true;
                        }
                    }
                    if (!tmp)
                    {
                        graph.AddEdge(city.name, neigh.name).Attr.Color = Msagl.Color.Gray;
                    }
                }
            }
            foreach (Edge e in this.infectedEdge)
            {
                graph.AddEdge(e.fromCity.name, e.toCity.name).Attr.Color = Msagl.Color.Red;
            }
            //bind the graph to the viewer 
            viewer.Graph = graph;

            //associate the viewer with the form 
            form.SuspendLayout();
            viewer.Dock = DockStyle.Fill;
            return viewer;
            //form.Controls.Add(viewer);
            //form.ResumeLayout();

            //show the form 
            //form.ShowDialog();
        }

        public string PrintInfectionPath()
        {
            string res = "";
            string envNewLine = Environment.NewLine;
            res += "Virus Path:" + envNewLine;
            //Console.WriteLine("Virus Path:");
            foreach (Edge cityEdge in this.infectedEdge)
            {
                res += "- ";
                res += cityEdge.printInfoEdge();
                res += envNewLine;
            }
            return res;
        }

        public void PrintInfo()
        {
            foreach (City city in this.cities.Values)
            {
                city.PrintInfo();
            }
        }

        public void PrintInfo(int t)
        {
            foreach (City city in this.cities.Values)
            {
                city.PrintInfo(t);
            }
        }

        public void loadFromFile(string population, string peta)
        {
            this.loadPopulationFromFile(population);
            this.loadPetaFromFile(peta);
        }

        private void loadPetaFromFile(string filename)
        {
            using (StreamReader sr = File.OpenText(filename))
            {
                string str = sr.ReadLine();
                while ((str = sr.ReadLine()) != null)
                {
                    string[] tmp = str.Split(' ');
                    this.cities[tmp[0]].SetConnectionRate(this.cities[tmp[1]], Double.Parse(tmp[2], System.Globalization.CultureInfo.InvariantCulture));
                }
            }
        }

        private void loadPopulationFromFile(string filename)
        {
            using (StreamReader sr = File.OpenText(filename))
            {
                string line = sr.ReadLine();
                string[] tmp = line.Split();
                int numCity = Int32.Parse(tmp[0]);
                this.startCity = tmp[1];

                while ((line = sr.ReadLine()) != null)
                {
                    tmp = line.Split(' ');
                    this.cities.Add(tmp[0], new City(tmp[0], Int32.Parse(tmp[1])));
                }
            }
        }
    }
}

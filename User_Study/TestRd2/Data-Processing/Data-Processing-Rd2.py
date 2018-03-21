# MVP:
# 3 keyboards, 
# ui control: 
# speed (wpm, adjWpm)and/or accuracy (totErrRate);
# FOA distribution ratio
# plot: sentenceNo, SD

%matplotlib notebook	
import csv
from __future__ import print_function
import ipywidgets as widgets
from IPython.display import display, clear_output
import matplotlib.pyplot as plt 
plt.style.use('ggplot')
import numpy as np

class Sentence:
	# class variables here
	def __init__(self, participant, sentenceNo, testing):
		# instance variables here
		self.participant = participant
		self.sentenceNo = sentenceNo
		self.testing = testing		# 0: practice; 1: test
		self.keyboard = 0			# 0: win EyeControl; 1: tobii WinControl; 2: tobii Dwell-free
		self.wpm = 0.0
		self.adjWpm = 0.0
		self.totErrRate = 0.0
		self.uncErrRate = 0.0
		self.corErrRate = 0.0

def readSentences(argFilename, argSentences):
	with open('Raw-Data/Logs/'+argFilename+'.csv') as csvDataFile:
		csvReader = csv.reader(csvDataFile)
		next(csvReader, None)
		next(csvReader, None)
		for row in csvReader:
			argSentences.append(Sentence(int(argFilename[0])-1, int(row[0]), int(row[1])))
			# exclude exceptions that the test participant didn't enter anything and skipped this sentence			
			if row[3] != 0:			
				sentences[-1].wpm = float(row[3])
				sentences[-1].adjWpm = float(row[4])
				sentences[-1].totErrRate = float(row[11])
				sentences[-1].uncErrRate = float(row[9])
				sentences[-1].corErrRate = float(row[10])				
				sentences[-1].keyboard = ord(argFilename[4])-65
		# print(len(sentences))

def clearCache():
	clear_output(wait=True)
	display(contrMetric, widgets.HBox([contrPtcp]))
	# clear data storage

def onChange_metric(b):
	if b['type']=='change' and b['name']=='value':
		plotsHub()

def onChange_ptcp(change):
	if change['type']=='change' and change['name']=='value':
		plotsHub()

def plotsHub():
	clearCache()
	# extract sentences according to participant selection
	for item in sentences:
		if item.testing == 1:
			if contrPtcp.value != 'All':
				index = (int(contrPtcp.value[1])-1)*amountKeyboard+item.keyboard
			else:
				index = item.participant*amountKeyboard+item.keyboard
			# print(index)
			perPtcpWpm[index].append(item.wpm)
			perPtcpAdjWpm[index].append(item.adjWpm)
			perPtcpTotErrRate[index].append(item.totErrRate)
			perPtcpUncErrRate[index].append(item.uncErrRate)
			perPtcpCorErrRate[index].append(item.corErrRate)
	filter()
	if 'Speed' == contrMetric.value:
		plotSpeed()
	elif 'Accuracy' == contrMetric.value:
		plotAccuracy()
	elif 'Speed & Accuracy' == contrMetric.value:
		plotSpeedNAccuracy()
	elif 'Learning' in contrMetric.value:
		plotLearningCurve()
	elif 'Speed vs. Accuracy' == contrMetric.value:
		plotSpeedVsAccuracy()	

def filter():
	print(perPtcpWpm)
	pass
	# remove outliers that out of 90%

def plotSpeed():
	fig, ax = plt.subplots()
	ax.set(title='Participant '+contrPtcp.value+' typing speed', ylabel='Entry Speed (wpm)')
	plt.ylim(-1,50)
	yWpm = [[] for i in range(amountKeyboard)]
	for index in range(len(perPtcpWpm)):
		for subIndex in range(len(perPtcpWpm[index])):
			yWpm[index%amountKeyboard].append(perPtcpWpm[index][subIndex])
	for i in range(amountKeyboard):
		plt.boxplot(yWpm[i], positions=[plotXPosition[i]], showfliers=True, patch_artist=True, boxprops=dict(facecolor=color[0]), medianprops=dict(linewidth=1, linestyle=None, color='yellow'))
	plt.xlim(min(plotXPosition)-1, max(plotXPosition)+1)	
	plt.xticks(plotXPosition, labelKeybd)
	fig.savefig('plotSpeed_'+contrPtcp.value+'.png', bbox_inches='tight')

def plotAccuracy():
	# after checking with keybd A, C, then copy speed code here
	pass

def plotSpeedNAccuracy():
	fig, ax = plt.subplots()
	ax.set(title='Participant '+contrPtcp.value+' typing speed with accuracy', ylabel='Entry Speed (wpm)')
	plt.ylim(-1,20)
	yWpm, sdTotErrRate = ([[] for i in range(amountKeyboard)] for j in range(2))
	for index in range(len(perPtcpWpm)):
		for subIndex in range(len(perPtcpWpm[index])):
			yWpm[index%amountKeyboard].append(perPtcpWpm[index][subIndex])
			sdTotErrRate[index%amountKeyboard].append(perPtcpTotErrRate[index][subIndex])
	for i in range(amountKeyboard):
		print(yWpm[i])
		print('---------')
		plt.errorbar(i+1,np.mean(yWpm[i]),np.mean(sdTotErrRate[i]),color=color[i],elinewidth=20,capsize=2)
	plt.xlim(min(plotXPosition)-1, max(plotXPosition)+1)	
	plt.xticks(plotXPosition, labelKeybd)
	fig.savefig('plotSpeedNAccuracy_'+contrPtcp.value+'.png', bbox_inches='tight')

def plotLearningCurve():
	pass

def plotAttention():
	pass

if __name__=="__main__":
	amountPtcp=6
	amountKeyboard=3
	sentences=[]
	perPtcpWpm, perPtcpAdjWpm, perPtcpTotErrRate, perPtcpUncErrRate, perPtcpCorErrRate = ([[] for j in range(amountPtcp*amountKeyboard)] for i in range(5))
	plotXPosition = [1,2,3]
	labelKeybd = ['Win10 Eye Control', 'Tobii Win Control', 'Tobii Dwell-free']
	color = ['gray', '#01ac66','green']


	filenames = ['1_kbB_logs', '2_kbB_logs', '3_kbB_logs', '2_kbA_logs_manual', '2_kbC_logs_manual']
	for item in filenames:
		readSentences(item, sentences)

	contrMetric = widgets.ToggleButtons(
		options=['Speed', 'Accuracy', 'Speed & Accuracy', 'Learning Curve', 'Attention Distribution'],
		description='Metric: ',
		value='Speed',
		disabled=False
		)
	contrPtcp = widgets.Select(
		options=['All', '#1', '#2', '#3', '#4', '#5', '#6'],
		description='Participant: ',
		value='All',
		disabled=False
		)
	display(contrMetric, widgets.HBox([contrPtcp]))
	contrMetric.observe(onChange_metric)
	contrPtcp.observe(onChange_ptcp)
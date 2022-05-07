# STM32F4-with-LM35-and-C#-GUI-programming

Notes:  
- To use in simulation mode, change Vref voltage to 5V.
- To use in real model, change Vref voltage to 3V.


# About this project
This project uses STM32F411 Discovery kit and two temperature sensors LM35 to measure the environmental temperature. The output data from LM35 is voltage (mV) and can be read with ADC channel of STM32F4, then we convert this value to temperature value and dislay it on LCD (I2C communication) by equation: 

![image](https://user-images.githubusercontent.com/104365389/167235424-ee78116e-79bf-4c6e-910c-6e7333f54149.png)

The data is also transmitted to computer with data acquisition (Winforms C#) through UART communication, so we can do a lot of stuff with GUI programming such as configuring, controlling, alarm warning when exceeding threshold, recording and loading data to file txt with chosen sample time.

# Connection
![image](https://user-images.githubusercontent.com/104365389/167236315-145083ff-0c3f-43c0-97f0-639b97ff0cda.png)

# Demo 
Link youtube: https://youtu.be/fz3rb3VAMGQ

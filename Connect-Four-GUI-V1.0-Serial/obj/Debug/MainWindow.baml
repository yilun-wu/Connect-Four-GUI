   M S B A M L   `   `   `  ����  ConnectFour    ConnectFour.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$&   Connect Four Bah-A-La-La-La V1.0��5   	    � SizeToContent$ WidthAndHeight=�62    � 
ResizeMode$ CanMinimize=�6Q   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5	      $	��480 �6    � ColumnDefinitions 5   
   �� 5      $��696.5 �6   �� 5      $��0 �6   �� 5      $��0 �6   .+�5   
   o� $	��Top=�6   $��0��6'   $��0��64   $��18��6D   $��	0,0,0.5,0q�6P   .@�5      m� -   5        $��
ui_mnuFile��5   %   $
��File��6   .@�5      m� -   5        $��	ui_mnuNew��5   -   $��New Game��6   6>   m� 5      -   5        $��
ui_mnuLoad��5   )   $
��Load��6   6;   m� 5      -   5        $��
ui_mnuSave��5   )   $
��Save��6   6;   5      5      �� 5   
   
�������$��1��6'   $��0��64   $��	0,0,0.5,0q�6D   .@�5      �� -   5        $��tb��5      
���   �	Scores:5      �� 5      -   5        $��ui_MousePosition��5      5      �� 5   
   -   5         ��Name$ 	theCanvas��5      $��2��6$   $��0��61   
�������$��	0,0,0.5,0q�6t   .+�5      �� $	��144��6    �� Left$	 554��6!   $��Baymax_Render.png��63   $
��Fill=�6N   
 �� Top$	 326��6]   $	��132��6n   5       5!      5"      
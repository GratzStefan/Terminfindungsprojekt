   M S B A M L   `   `   `  ����L  HTerminfindungsapp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:TerminfindungsappTerminfindungsapp  5   	   &     Terminfindungsapp.RegisterWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:Terminfindungsapp     5   	     � Title$  SuperPlaner��5   	    � Icon$ Images/icon.png��6   $	��450��5	   	   $	��800��6   .��5
      � ���#002159 � ColumnDefinitions 5   
   �� 5      $	��35* �6   �� 5      $	��30* �6   �� 5      $	��35* �6   .+�5   
   � $��1��6   $��	0,20,0,20q�6    � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $
��100* �6    �� 5      $
��Auto �6    .+�5      �� $��0��6   $��Center=�6!   $��Center=�6>   $��SIGN UP��6Y   $
��Bold"�5      $��35(�6   $��15q�6-   ���#FF595A6:   �� 5      $��1��6   .+�5      �� $��	FIRSTNAME��6   $
��Bold"�6,   $��10,0,0,0q�6>   ���#CAC0B3$��15(�6e   �� 5      -   5        $��txtFirstname��5       �� TextWrapping$
 Wrap=�6/   $��
15,0,10,10q�6C   ���#CAC0B3$��15(�6l   �� 5      $��LASTNAME��6   $
��Bold"�6+   $��10,0,0,0q�6=   ���#CAC0B3$��15(�6d   �� 5      -   5        $��txtLastname��5      $
 Wrap=�6.   $��
15,0,10,10q�6B   ���#CAC0B3$��15(�6k   �� 5       $��USERNAME��6   $
��Bold"�6+   $��10,0,0,0q�6=   ���#CAC0B3$��15(�6d   �� 5!      -   5        $��txtUsername��5!      $
 Wrap=�6.   $��
15,0,10,10q�6B   ���#CAC0B3$��15(�6k   �� 5#      $��PASSWORD��6   $
��Bold"�6+   $��10,0,0,0q�6=   ���#CAC0B3$��15(�6d   G� 5$      -   5        $��txtPassword��5$      $��
15,0,10,10q�61   ���#CAC0B3$��15(�6Z   5%      �� 5&      $��2��6   .+�5'      �� -   5        $��btnRegister��5'      $��REGISTER��6,   $
��Bold"�6?   $��10q�6Q   $��5q�6]   ���#FF595A$��0q�6�   $��15(�6�   �� 5(      -   5        $��btnLogin��5(      $��Do you want to LOGIN��6)   $
��Bold"�6H   $��
10,0,10,10q�6Z   $��5q�6n   
������ ���#CAC0B3���#CAC0B3$��15(�6�   5)      5*      5,      5-      